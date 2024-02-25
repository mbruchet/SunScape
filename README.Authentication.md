# Authentication module
## Introduction
This module focuses on implementing server-side authentication using *Microsoft.AspNetCore.Identity.EntityFrameworkCore*. 
We'll explore creating a login form that utilizes email and password for user authentication. 
A significant portion will be dedicated to implementing two-factor authentication, enhancing the security of our application.

On the client-side, we'll delve into Microsoft.AspNetCore.Components.WebAssembly.Authentication, discussing how to implement AuthenticationStateProvider and a redirection component for handling unauthenticated users or expired sessions.

Additionally, we'll cover the integration of Single Sign-On (SSO) with social media accounts like Facebook and Google, providing a streamlined login experience.

Finally, an important aspect for enterprises, the integration with Microsoft Identity Platform for authentication using Azure AD, will be discussed, offering a robust solution for managing user identities and permissions.

This README outlines the key components and steps involved in securing your application, providing a solid foundation for both server and client-side authentication strategies.

## Packages required
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Relational
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore

## Database
1. Create entity to store Application user profile: IdentityUser
2. Create DbContext 
3. Register the DbContext
4. Add ConnectionString in settings (it should recommanded to use keyvault to store any credentials or ConnectionString. Locally you can use User secrets.json file)
5. Add Migration

## Implement Code
Following the code todo list

## Email Sender service
# Configuring Email Services with ASP.NET Identity 8

ASP.NET Identity 8 provides versatile options for email service integration. Here, we'll cover three primary methods: generating email message files, using SMTP with authentication, and leveraging SendGrid.

## Generating Email Message Files
To create a service that generates email message files, you can utilize the `SmtpClient`'s `PickupDirectoryLocation` property.

* Use FileEmailService to generate file
* Use SmtpEmailService to send by Smtp and use EmailStmp Section in AppSettings.$Env.json to set your Server name and portion
* Use secrets.json file to store Smtp UserName and password
* Use SendGridEmailService to send by SendGrid SAAS Service. Use EmailSendGrid Section AppSettings.$Env.json

## Create Page Restricted access
You can use @attribute [Authorize] to restrict access on a full page or <AuthorizeView> to a specific zone
View AuthServer.razor for server side or AuthClient.razor for client side

## Token Management in ASP.NET Core Identity
ASP.NET Core Identity provides a comprehensive system for managing user authentication and authorization. A crucial part of this system is the management of tokens for various purposes such as data protection, email confirmation, SMS verification, and two-factor authentication (2FA) codes. The AddDefaultTokenProviders method plays a key role in setting up these token providers.

### Overview of AddDefaultTokenProviders
When configuring ASP.NET Core Identity in your application, calling AddDefaultTokenProviders adds the default implementations for token generation and validation. This method is essential for enabling features like email confirmation, password reset, and 2FA.

```services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();```

#### Default Token Providers
ASP.NET Core Identity includes several default token providers:

##### Data Protection Token Provider
Purpose: Used for generating tokens for password resets, email confirmations, and other security-related operations.
Implementation: Utilizes the ASP.NET Core Data Protection API to encrypt and decrypt tokens.

##### Email Token Provider
Purpose: Specifically used for generating tokens sent via email, such as email confirmation tokens.
Implementation: Generates tokens that are shorter-lived, suitable for email verification processes.

##### Phone Number Token Provider
Purpose: Generates tokens for phone number verification via SMS.
Implementation: Creates tokens that are ideal for the short-lived nature of SMS verification codes.

##### Authenticator Token Provider
Purpose: Supports tokens for 2FA, typically used in conjunction with an authenticator app.
Implementation: Generates time-based one-time passwords (TOTPs) that are valid for a short period, enhancing security for 2FA.

##### Customizing Token Providers
While the default token providers cover a wide range of use cases, you might encounter scenarios requiring customized token generation or validation logic. ASP.NET Core Identity allows for the customization or replacement of these providers to meet specific functional requirements.

To customize a token provider, you can implement your own provider by inheriting from IUserTwoFactorTokenProvider<TUser> for 2FA tokens or IUserTokenProvider<TUser, TKey> for other types of tokens. After implementing your custom provider, register it in the service configuration:

```services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<CustomTokenProvider<ApplicationUser>>("CustomName");```

#### Conclusion
Token management is a vital aspect of securing applications and managing user authentication states. ASP.NET Core Identity's AddDefaultTokenProviders method, along with the flexibility to customize token providers, offers a robust framework for handling various token-based operations efficiently and securely.

This template provides a concise yet comprehensive overview of token management in ASP.NET Core Identity, suitable for inclusion in a project's README file on GitHub. It outlines the purpose and implementation of the default token providers and explains how to customize token providers for specific needs.

