# EviCRM Server
## Серверная часть EviCRM, настольного инструмента менеджера

Сильно упрощённая CRM система, рассчитанная на пользователя, который никогда раньше не работал ни с чем подобным

![](https://evicrm.store/Screenshot_7.png)

- Смотрите как работают ваши сотрудники - аналитика статистики пользовательских сеансов (при помощи [![Mipko](https://www.mipko.ru/)](https://www.mipko.ru/))
- Календарь для заметок
- Умный телеграм бот Александра всегда поможет в любом вопросе - она управляет всей системой и напоминает о событиях в Телеграм
- Видеоконференции и совместное редактирование документов
- Встроенный сильно упрощённый задачник, который позволяет максимально просто и понятно следить за всеми изменениями в задаче
- Журнал для отметок\оценок\заметок о работе сотрудника
- Локальный, зашифрованный сквозным шифрованием, чат
- Корпоративная почта
- Хранилище ваших файлов (с поддержкой SFTP)
- Рабочий стол (RDP сеанс в браузере при помощи [![RPDWeb](https://client.wvd.microsoft.com/arm/webclient/index.html)](https://client.wvd.microsoft.com/arm/webclient/index.html))
- Новостной локальный портал
- Интеграция карт (при помощи [![Leaflet Maps || OSM](https://leafletjs.com/)](https://leafletjs.com/))

## Особенности
![](https://evicrm.store/Screenshot_8.png)
- Максимально упрощённый интерфейс
- Загрузка файлов без ограничений на их размер
- Адаптировано под мобильные устройства

> Проект был написан за считанные пару месяцев, ещё в стадии разработки и позиционируется как решение "для дружественной компании". Поставляется as is

Решение идеально для организации работы команды продаж малого бизнеса, вам не придётся обучать сотрудников сложным CRM систем и мониторить активность и разрешать возникшие проблемы может всего 1 админ\секретарь

## Стек технологий

EviCRM состоит из нескольких частей, обобщая стек выглядит так:

- [.NET Core Blazor Server] - основа, движок динамического веб-приложения!
- [.NET Core Razor Pages] - статичные веб-страницы, например, авторизация
- [Nuget: Havit] - Havit UX\UI Framework для красивого фронта
- [Nuget: Bootstrap] - основа фронта
- [Nuget: MySQL Connector] - основа для интеграции любой mysql-совместимой базы данных. Например, MySQL или MariaDB. Запросы отправляются в сыром виде, получая выборку с конечной базы данных, в последствии анализируя в коде бэкэнда.
- [ORM: Entity Framework && ASP.NET Authorization] - ORM для работы с базами данных, через EF построена авторизация
- [jQuery и множество JS библиотек зависимостей] - жизнь клиентского интерфейса
- [LeafletJS](https://leafletjs.com/) - Leaflet JS карты
- И многое многое другое

## Установка

Необходимо склонировать репозиторий и открыть проект в Visual Studio 2022
Для запуска на конечном сервере под управлением Windows 7+ \\ Windows Server 2012+ ничего больше не требуется

Для запуска в окружении Linux потребуется развернуть образ docker или запустить нативно:

```sh
dotnet ./EviCRM.dll
```

Можно запустить используя mono:
```sh
mono ./EviCRM.exe
```

Для конфигурации SignalR требуется настроить nginx:

```nginx
server {
    server_name  $ADDR;
  
     location / {
        proxy_pass         http://127.0.0.1:5000/;
        proxy_http_version 1.1;
        proxy_set_header   Host $host;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_buffering off;
    }

    root /var/www/html;
    add_header Access-Control-Allow-Origin *;
    error_page 405 =200 $uri;
}

    server {
        listen                    443 ssl http2;
        server_name               $URL
        ssl_session_timeout       1d;
        ssl_protocols             TLSv1.2 TLSv1.3;
        ssl_prefer_server_ciphers off;
        ssl_ciphers               ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-ECDSA-CHACHA20-POLY1305:ECDHE-RSA-CHACHA20-POLY1305:DHE-RSA-AES128-GCM-SHA256:DHE-RSA-AES256-GCM-SHA384;
        ssl_session_cache         shared:SSL:10m;
        ssl_session_tickets       off;
        ssl_stapling              off;

        add_header X-Frame-Options DENY;
        add_header X-Content-Type-Options nosniff;

      location /
      {
      proxy_pass         https://127.0.0.1:5001/;
      proxy_http_version 1.1;
        proxy_set_header   Host $host;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_buffering off;
        
      }
    root /var/www/html;
    add_header Access-Control-Allow-Origin *;
}

```

## Разработка
Проект находится в разработке, портировалась старая версия с движка на Razor Pages, планируется публикация под лицензией открытого кода в ближайшее время
