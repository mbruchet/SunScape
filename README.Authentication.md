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


### Authenticate with SSO
#### What is Single Sign-On (SSO) Authentication?
Single Sign-On (SSO) is an authentication process that allows a user to access multiple applications or systems with one set of credentials. 
This means that the user logs in once and gains access to all associated systems without being prompted to log in again at each of them. 
SSO simplifies the user experience by reducing the number of times a user has to log in and thereby minimizing the cognitive load of managing multiple usernames and passwords.

#### The Importance of SSO
The significance of SSO lies in its ability to improve both security and user experience. 
From a security perspective, SSO reduces the likelihood of password fatigue, where users reuse the same password across multiple sites, making it easier for attackers to gain unauthorized access. 
It also decreases the risk of phishing attacks, as users are less likely to enter credentials into fraudulent websites. 

From a user experience standpoint, SSO streamlines the login process, making it faster and more convenient to access various services, which is particularly beneficial in environments where users need to interact with multiple applications regularly, such as in corporate settings.

#### Modify Authentication with Social Network SSO
To modify an application's basic authentication mechanism to support SSO with providers like Google, Facebook, you typically need to follow these steps:

1. Choose an SSO Strategy: 
Decide whether to use a dedicated SSO solution or integrate SSO capabilities directly into your application. 
Dedicated SSO solutions might include identity providers (IdPs) like Okta or Auth0, which can simplify the integration process.

2. Register Your Application with the Providers:
For Google and Facebook, or any other social network account, you'll need to create an application in their developer consoles, configure the OAuth 2.0 settings, and obtain client IDs and secrets.

##### For Google: https://console.cloud.google.com
Here's a step-by-step guide in English on how to configure Google authentication for your application:

###### Step 1: Create a Google Cloud Project
Navigate to the Google Cloud Console: Go to console.cloud.google.com and sign in with your Google account.
Create a New Project: Click on the project selector at the top of the page, then click "New Project". Give your project a name and, if necessary, associate it with an organization. Click "Create".

###### Step 2: Set Up OAuth 2.0 Credentials
Open the Credentials Page: In the Google Cloud Console, navigate to "APIs & Services" > "Credentials".
Configure the OAuth Consent Screen: Before creating credentials, you need to configure the OAuth consent screen. Click on "OAuth consent screen" and fill in the required fields, such as the user support email, app name, and developer contact information. Save your changes.
Create OAuth 2.0 Credentials: Click on "Create Credentials" at the top of the page and select "OAuth client ID".
Application Type: Choose "Web application".
Name: Give your OAuth 2.0 client a name.
Authorized JavaScript origins: Add the origins from which your app will be allowed to make requests (e.g., https://www.yourdomain.com).
Authorized redirect URIs: Add the URIs to which Google will redirect users after they authenticate. For ASP.NET Core applications, this is typically your application's URL followed by /signin-google (e.g., https://www.yourdomain.com/signin-google).
Create: Click "Create". You will be presented with your client ID and client secret. Note these down as you will need them to configure Google authentication in your application.

###### Step 3: Configure Google Authentication in Your Application
Install the Necessary Package: Ensure you have the Google authentication package installed in your ASP.NET Core application. If not, you can install it via NuGet Package Manager or the Package Manager Console:

mathematica
Copy code
Install-Package Microsoft.AspNetCore.Authentication.Google
Modify Program.cs: In your Program.cs file, add the Google authentication service in the builder.Services.AddAuthentication().AddGoogle method

Replace YOUR_CLIENT_ID and YOUR_CLIENT_SECRET with the values obtained from the Google Cloud Console.

Configure the Redirect URI: Ensure the redirect URI in your application matches one of the authorized redirect URIs you specified in the Google Cloud Console.

###### Step 4: Test the Authentication Flow
Run Your Application: Start your application and navigate to the login page.
Select Google to Sign In: Your application should now offer an option to sign in with Google. Select this option.
Complete the Authentication: You will be redirected to Google's sign-in page. After signing in, Google will redirect you back to your application, completing the authentication process.

###### Step 5: Handle the Authentication Response
Ensure your application correctly handles the authentication response from Google, creating a new user account or linking to an existing one as necessary. 
This typically involves configuring the external login callback path in your application to process the sign-in response.

By following these steps, you'll have configured Google authentication for your application, allowing users to sign in with their Google accounts seamlessly.

### Authenticate with Two factor
Two-factor authentication (2FA) with Authenticator application enhances security by requiring two types of identity proofs from the user before granting access to a resource. Here are the key steps and principles:

- Installation and Setup: Users install Authenticator application on their mobile device. When setting up an account for 2FA, the service generates a QR code that the user scans with the app. This process links the user's account to Authenticator application.
- Code Generation: Authenticator application uses a Time-based One-Time Password (TOTP) algorithm to generate 6 to 8-digit security codes that change every 30 seconds.
- Authentication: When logging into a service protected by 2FA, the user enters their username and password (first factor) and then the code currently displayed by Authenticator application (second factor). Only the correct combination of both allows access.
- Enhanced Security: Even if an attacker obtains the password, it's nearly impossible for them to access the account without the second factor generated on the user's device.
- Backup and Recovery: It's crucial to backup the recovery codes provided during the 2FA setup to avoid losing access to accounts if the mobile device is lost or inoperable.

#### Priority Actions:

1. Activate 2FA with Authenticator application for all critical accounts, especially emails, cloud platforms, and management systems.
2. Educate teams on the importance of 2FA and security best practices.
3. Establish access management and recovery protocols to minimize the risks of access loss.
4. By adopting Authenticator application as a 2FA solution, you add an essential layer of security, thereby protecting sensitive data and critical infrastructures from unauthorized access

### Forgot Password
The "Forgot Password" function is a critical feature in modern web applications and systems, providing a way for users to recover access to their accounts when they forget their passwords. This functionality is essential for maintaining user access and security while also reducing the administrative burden of manually resetting passwords. Here's an overview of how it typically works and some best practices:

How It Works
1. Request Phase: The user clicks on a "Forgot Password" link on the login page and is prompted to enter their email address or username associated with their account.
2. Verification Phase: The system verifies if the provided email address or username exists in the database. If it does, the system proceeds to the next step. It's a good practice not to disclose whether the email address was found in the system to prevent information leakage.
3. Email with Reset Link: The system sends an email to the user's registered email address. This email contains a secure link that the user can click to reset their password. This link is typically time-limited and may expire after a set period (e.g., 1 hour) for security reasons.
4. Password Reset Page: Clicking the link in the email directs the user to a password reset page where they can enter a new password. This page should also enforce strong password standards.
5. Confirmation: After the user sets a new password, the system updates the user's account with the new password, and the user can now log in using it. The system may also notify the user that their password has been changed for additional security.

#### Best Practices
- Secure Token: The reset link should include a secure, unique token that is difficult to guess. This token should be stored securely and compared when the reset link is used.
- Expiration Time: The reset token should expire after a short duration or after it has been used, whichever comes first, to reduce the window of opportunity for an unauthorized reset.
- Rate Limiting: Implement rate limiting on password reset attempts to prevent abuse of the feature.
- HTTPS: Ensure that the password reset page and all related processes are served over HTTPS to protect the user's information.
- Logging and Monitoring: Log password reset attempts and monitor for any unusual activity, such as a high volume of requests, which could indicate an attack.
- User Notification: Notify the user via email when their password has been changed, and provide a way to report it if the change was not initiated by them.

Implementing a "Forgot Password" function requires careful consideration of security and usability to ensure that users can easily recover access to their accounts without compromising their security.
