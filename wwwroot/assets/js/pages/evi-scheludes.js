"use strict";
var ScheduleList = [],
    SCHEDULE_CATEGORY = ["milestone", "task"];

function ScheduleInfo() {
    this.id = null, this.calendarId = null, this.title = null, this.body = null, this.isAllday = !1, this.start = null, this.end = null, this.category = "", this.dueDateClass = "", this.color = null, this.bgColor = null, this.dragBgColor = null, this.borderColor = null, this.customStyle = "", this.isFocused = !1, this.isPending = !1, this.isVisible = !0, this.isReadOnly = !1, this.goingDuration = 0, this.comingDuration = 0, this.recurrenceRule = "", this.state = "", this.raw = {
        memo: "",
        hasToOrCc: !1,
        hasRecurrenceRule: !1,
        location: null,
        class: "public",
        creator: {
            name: "",
            avatar: "",
            company: "",
            email: "",
            phone: ""
        }
    }
}

function generateTime(e, a, o) {
    var n = moment(a.getTime()),
        i = moment(o.getTime()),
        t = i.diff(n, "days");
    e.isAllday = chance.bool({
        likelihood: 30
    }), e.isAllday ? e.category = "allday" : chance.bool({
        likelihood: 30
    }) ? (e.category = SCHEDULE_CATEGORY[chance.integer({
        min: 0,
        max: 1
    })], e.category === SCHEDULE_CATEGORY[1] && (e.dueDateClass = "morning")) : e.category = "time", n.add(chance.integer({
        min: 0,
        max: t
    }), "days"), n.hours(chance.integer({
        min: 0,
        max: 23
    })), n.minutes(chance.bool() ? 0 : 30), e.start = n.toDate(), i = moment(n), e.isAllday && i.add(chance.integer({
        min: 0,
        max: 3
    }), "days"), e.end = i.add(chance.integer({
        min: 1,
        max: 4
    }), "hour").toDate(), !e.isAllday && chance.bool({
        likelihood: 20
    }) && (e.goingDuration = chance.integer({
        min: 30,
        max: 120
    }), e.comingDuration = chance.integer({
        min: 30,
        max: 120
    }), chance.bool({
        likelihood: 50
    }) && (e.end = e.start))
}

function generateNames() {
    for (var e = [], a = 0, o = chance.integer({
            min: 1,
            max: 10
        }); a < o; a += 1) e.push(chance.name());
    return e
}

function generateRandomSchedule(e, a, o) {

}

function generateSchedule(n, i, t) {
  
}