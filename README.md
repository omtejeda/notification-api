# Notification API
___
Notification API is an API designed to handle everything related to sending notifications from any platform/application within your applications ecosystem.

## Table of Contents
___
* [Features](#features)
* [Technologies](#technologies)
* [Database](#database)
* [Platforms](#platforms)
* [Providers](#providers)
* [Templates](#templates)
* [Notifications](#notifications)
* [Frequently Asked Questions](#frequently-asked-questions)
* [License](#license)

## Features
___
- It is generic
- Fully configurable
- Has its own database
- Can be reused by any application


## Technologies
___
- .NET 6.0
- MongoDB
- Docker
- Kubernetes

## Database
___
Notification API has its own database, a NoSQL database, MongoDB.
The following information is stored in this database:
- Collection __Platforms__: stores the platforms/applications configured to use the API
- Collection __Providers__: stores the email providers that have been configured for each platform
- Collection __Templates__: stores the templates that have been configured for each platform
- Collection __Notifications__: stores the tracking of all notifications that have been sent and/or attempted to be sent

### Platforms
___
In order to start making use of Notification API, an API Key is required.

Each platform/application that will use the Notification API must register using the following operation:

__Route__: /api/v1/notification/platforms  
__HTTP Verb__: POST  
__Content-Type__: application/json

You must specify:
- __name__: Name of the platform/application
- __description__: Description of the platform/application

As a result, an API Key is generated, which your platform/application will use to interact with the Notification API. Keep it in a safe place!


__Registering a Platform__
```json
{
    "name": "ClientExpensesApp",
    "description": "Mobile app used to track client's expenses"
}
```



## Providers
___
In order to send notifications, the API needs to know which email provider to use.

__But... what is a Provider?__
> A provider is the service (either internal or third party) for sending emails/SMS that will be used for delivery. Providers are dynamically configured in the API. You can configure providers like SendGrid and SMTP.



__Registering a SendGrid Provider__
```json
{
    "name": "ClientExpensesSendGrid",
    "type": "SendGrid",
    "settings": {
        "sendGrid": {
            "fromEmail": "noreply@clientexpenses.com",
            "fromDisplayName": "Client Expenses",
            "apiKey": "yourSendGridApiKey"
        }
    },
    "isActive": true,
    "isPublic": false
}
```

__Registering a SMTP Provider__
```json
{
    "name": "ClientExpensesOutlook",
    "type": "SMTP",
    "settings": {
        "smtp": {
            "fromEmail": "notifications@clientexpenses.com",
            "fromDisplayName": "Client Expenses",
            "host": "outlook.office365.com",
            "port": 993,
            "password": "yourSMTPServerPassword"
        }
    },
    "isActive": true,
    "isPublic": true
}
```


## Templates
___
The API provides an operation to register the templates that your application may send.

__Route__: /api/v1/notification/templates
__HTTP Verb__: POST
__Content-Type__: application/json

You must specify:
* __name__: template name
* __platformName__: name of the platform/application the template will be associated with
* __notificationType__: type of notification for which it will be used. Possible values: Email / SMS
* __language__: template language (es / en; Spanish and English respectively)
* __subject__: subject of the email that will be sent with this template
* __content__: HTML content of the template
* __metadata []__: specify the placeholders required by the template, which must be sent in the request to be replaced in the HTML at runtime.
		- ___key___: placeholder name
		- ___description___: placeholder description
		- ___isRequired___: indicates whether it is mandatory

Registering a User Registration Template__
```json
{
    "name": "UserRegistration",
    "notificationType": "Email",
    "subject": "User registration confirmation",
    "language": "en",
    "content": "Hi $[Username], thanks for your registration in our app!",
	"metadata": [{
        "key": "Username",
        "description": "User's name",
        "isRequired": true
    }]
}
```

## Notifications
___
The tracking of all notifications that have been sent and/or attempted to be sent, whether successful or not, is stored.

When sending a notification, a __notificationId__ is returned.
This __notificationId__ can be used to:
- Query the notification
- Resend the notification

__Query Notifications__
You can query the notifications sent or delivery attempts and view information such as:

- __notificationId__: Unique identifier of the notification
- __toDestination__: Email address/phone number of the recipient
- __templateName__: Name of the template used
- __platformName__: Name of the platform
- __date__: Date and time the request was made
- __success__: Indicates whether it was successful or failed
- __request__: Request body provided for sending the notification
- __parentNotificationId__: If the notification was resent from another, specifies the notificationId of the parent notification


__Resend Notifications__

You can resend notifications by providing the notificationId of the notification you want to resend.

As a result, the notification is sent again with a new notificationId, and this new notification is linked to the original notification with the __parentNotificationId__ property.

## Frequently Asked Questions
__What types of notifications are sent?__
> Currently, notifications via email and SMS.

__Who can use the API?__
> Any application that needs to send notifications.

__Do notifications have to be sent with templates?__
> Yes, they must be sent with templates previously configured in the same API. See the Templates section.


## License
___
Designed and developed by:
> Omarky Tejeda | Software Engineer


**Open Source?.. Why not?**