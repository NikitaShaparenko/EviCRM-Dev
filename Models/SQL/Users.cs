using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EviCRM.Server.Models.SQL
{
    public class Users
    {
        [Key]
        public int Users_ID { get; set; } //ID пользователя

        [Required(ErrorMessage = " Введите логин")]
        [MaxLength(128)]
        public string Users_login { get; set; } //Логин

        [Required(ErrorMessage = " Введите пароль")]
        [MaxLength(512)]
        public string Users_password_sha512 { get; set; } //Хеш пароля в SHA-512

        [Required(ErrorMessage = " Введите ник пользователя Телеграм")]
        [MaxLength(64)]
        public string Users_telegram { get; set; } //Ник в Телеграме

        [Required(ErrorMessage = " Введите номер телефона")]
        [MaxLength(16)]
        public string Users_phone_number { get; set; } //Телефонный номер

        [Required(ErrorMessage = "Ожидается имя")]
        [MaxLength(64)]
        public string Users_name_first { get; set; } //Имя пользователя

        [Required(ErrorMessage = "Ожидается отчество")]
        [MaxLength(64)]
        public string Users_name_middle { get; set; } //Отчество пользователя

        [Required(ErrorMessage = "Ожидается фамилия")]
        [MaxLength(64)]
        public string Users_name_last { get; set; } //Фамилия пользователя

        [Required(ErrorMessage = "Введите день рождения!")]
        public DateTime Users_birthday { get; set; } //День рождения

        [Required]
        public DateTime Users_registrationdate { get; set; } //Дата регистрации

        [Required(ErrorMessage = "Введите название отдела")]
        [MaxLength(64)]
        public string Users_department { get; set; } //Департамент

        [Required(ErrorMessage = "Введите должность")]
        [MaxLength(64)]
        public string Users_position { get; set; } //Должность

        [Required(ErrorMessage = "Введите комментарий")]
        [MaxLength(256)]
        public string Users_comment { get; set; } //Комментарий
    }
}
