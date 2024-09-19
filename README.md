# Notification API
___
Notification API is an REST API designed to manage the delivery of notifications across any platform or application within your ecosystem.

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
- Highly flexible and configurable
- Dedicated database for storage
- Generic and flexible design for use in many different applications
- Easily integrable with any application


## Technologies
___
- .NET 8.0
- MongoDB
- Docker
- Kubernetes

## Database
___
The Notification API utilizes its own NoSQL database, MongoDB, which includes the following collections:

- __Platforms__: Stores information about the platforms or applications configured to use the API.
- __Providers__: Contains details of the delivery providers configured for each platform.
- __Templates__: Maintains the templates configured for each platform and notification type.
- __Notifications__: Tracks all notifications that have been sent or attempted.

### Platforms
___
To use the Notification API, an API Key is required.

Each platform or application that intends to use the Notification API must be registered via the following endpoint:

__Route__: /api/v1/notification/platforms
__HTTP Verb__: POST  
__Content-Type__: application/json

You must provide the following details:
- __name__: The name of the platform or application
- __description__: A description of the platform or application

After successful registration, an API Key will be provided for accessing the Notification API. Be sure to keep this key safe.

__Example: Registering a Platform__
```json
{
    "name": "ClientExpensesApp",
    "description": "Mobile app used to track client's expenses"
}
```



## Providers
___
In order to send notifications, the API needs to know which deelivery provider to use.

__But... what is a Provider?__
> A provider is a service, either internal or third-party, used for sending emails, SMS, or other notifications. The API allows for dynamic configuration of these providers. You can set up providers such as SendGrid, SMTP or REST API calls.



__Example: Registering a SendGrid Provider__
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

__Example: Registering an SMTP Provider__
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

The request must include:
* __name__: The name of the template
* __platformName__: The platform or application associated with the template
* __notificationType__: The type of notification (Email, SMS)
* __language__: The language of the template (e.g., "es" for Spanish, "en" for English)
* __subject__: The subject line of the email (if it's an email)
* __content__: The content of the template (may be a simple text or HTML)
* __metadata []__: An array specifying placeholders required by the template, which must be replaced at runtime. Each placeholder should include:
	- ___key___: The placeholder name
	- ___description___: A description of the placeholder
	- ___isRequired___: Indicates whether the placeholder is mandatory

__Example: Registering a User Registration Template__
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
The API tracks all notifications that have been sent or attempted, regardless of success or failure.

When sending a notification, a __notificationId__ is returned, which can be used to:
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

To resend a notification, provide the notificationId of the original notification. The API will resend the notification with a new __notificationId__ and link it to the original notification using the __parentNotificationId__ property.

## Frequently Asked Questions
__What types of notifications are sent?__
> Currently, the API supports sending notifications via email natively, while SMS, push notifications, and other types are handled through providers configured as HTTP clients (REST API calls).

__Who can use the API?__
> Any application that needs to send notifications.

__Do notifications have to be sent with templates?__
> Yes, notifications must be sent using pre-configured templates. Refer to the Templates section for more information.


## License
___
Designed and developed by:
> Omarky Tejeda | Software Engineer


**Open Source?.. Why not?**