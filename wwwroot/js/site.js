// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


function(){

   
    var num = getRandomInt(19);
	console.log('random variable is equal = ' + num);
    switch (num) {
        case 0:
            document.getElementById("div.quotes").innerHTML = "Я верю в силу, когда что - то идет не так.Завтра новый день, и я верю в чудеса.» Одри Хепберн";
            break;
        case 0:
            document.getElementById("div.quotes").innerHTML = "Никогда не извиняйтесь за эмоциональность.Пусть это будет знаком того, что у вас доброе сердце, и вы не боитесь его показать.» Бриджит Николь";
            break;
        case 1:
            document.getElementById("div.quotes").innerHTML = "Сомнение убивает, поэтому вы всегда должны точно знать, кто вы и за что боретесь».Дженнифер Лопез";
            break;
        case 2:
            document.getElementById("div.quotes").innerHTML = "Принимайте критику, но не принимайте ее на свой счет.Если в нем есть капля правды — слушай, если нет — не подпускай к себе ».Хиллари Клинтон";
            break;
        case 3:
            document.getElementById("div.quotes").innerHTML = "Лучшая защита для каждой женщины — это мужество».Элизабет Кэди Стэнтон";
            break;
        case 4:
            document.getElementById("div.quotes").innerHTML = "Никто не может заставить вас чувствовать себя неполноценным без вашего согласия».Элеонора Рузвельт";
            break;
        case 5:
            document.getElementById("div.quotes").innerHTML = "Будь умной девушкой, женщиной со своим собственным мнением и дамой с достоинством».Автор неизвестен";
            break;
        case 6:
            document.getElementById("div.quotes").innerHTML = "Женщина, не ищущая ни у кого одобрения, — самый опасный человек в мире».Махадева Наджуми";
            break;
        case 7:
            document.getElementById("div.quotes").innerHTML = "Если хочешь встретить любовь всей своей жизни — посмотри в зеркало».Байрон Кэти";
            break;
        case 8:
            document.getElementById("div.quotes").innerHTML = "Нет косметики для красоты лучше счастья».Мария Митчел";
            break;
        case 9:
            document.getElementById("div.quotes").innerHTML = "Если вы сосредоточитесь на том, что у вас есть, вы всегда получите больше.Если вы думаете о том, чего нет, вам никогда не будет достаточно ».Опра Уинфри";
            break;
        case 10:
            document.getElementById("div.quotes").innerHTML = "Сегодняшнее« нет »не означает завтрашний отказ».Ивонн Орджи";
            break;
        case 11:
            document.getElementById("div.quotes").innerHTML = "Мы должны признать, что не всегда поступаем правильно.Неудача — это не противоположность успеху, это его часть ».Арианна Хаффингтон";
            break;
        case 12:
            document.getElementById("div.quotes").innerHTML = "Сильная женщина улыбается утром, как будто не плакала прошлой ночью».Гарриет Морган";
            break;
        case 13:
            document.getElementById("div.quotes").innerHTML = "Любая женщина, которая понимает, как управлять домом, ближе всего к пониманию того, как управлять страной».Маргарет Тэтчер";
            break;
        case 14:
            document.getElementById("div.quotes").innerHTML = "Давайте позаботимся о том, чтобы у каждой из миллионов женщин и девочек была возможность, которую они заслуживают».Хиллари Клинтон";
            break;
        case 15:
            document.getElementById("div.quotes").innerHTML = "Только мои ожидания достойны того, чтобы оправдывать их».Мишель Обама";
            break;
        case 16:
            document.getElementById("div.quotes").innerHTML = "Феминизм не сделает женщин сильнее, потому что они уже сильны.Задача феминизма — заставить мир признать эту силу ».Джей Ди Андерсон(Джина Данн)";
            break;
        case 17:
            document.getElementById("div.quotes").innerHTML = "Нет нужды приручать волчицу в себе только потому, что ты встретил кого - то, кто не мог с тобой справиться».Belle Estreller";
            break;
        case 18:
            document.getElementById("div.quotes").innerHTML = "Красота исходит изнутри.Если вы счастливы и живете полной жизнью, даже когда все идет не так, внешне вы становитесь красивее ».Фейт Хи";
            break;
       
        default:
             document.getElementById("div.quotes").innerHTML = "«Я верю в силу, когда что - то идет не так.Завтра новый день, и я верю в чудеса.» Одри Хепберн";
             break;
    }

};

function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}