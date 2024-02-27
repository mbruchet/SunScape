# Introduction to Blazor Web App 8
Blazor Web App 8 is the latest iteration of the innovative web framework by Microsoft, designed to enable developers to build interactive and modern web applications using C# and .NET instead of JavaScript. This framework allows the development of client-side web applications that run directly in the browser using WebAssembly, as well as server-side applications that utilize SignalR for UI updates.

## What's New in Blazor Web App 8
Blazor Web App 8 introduces several enhancements and new features aimed at improving performance, development experience, and integration capabilities. These updates include:

* Performance Improvements: Enhanced rendering speed and reduced application size for faster load times.
* New Component Features: Additional component capabilities for building richer user interfaces.
* Improved Development Tools: Enhanced tooling support in Visual Studio and Visual Studio Code for a more efficient development process.
* Advanced Integration Options: Better integration with modern JavaScript libraries and APIs, allowing for more flexible and powerful web applications.

## Creating a Project
To create a Blazor Web App in Visual Studio 2022, first ensure you have the latest version of Visual Studio with the ASP.NET and web development workload installed. Then, create a new project and select the "Blazor Web App" template, which supports both interactive server-side rendering (SSR) and client-side rendering (CSR). This template is recommended for learning about Blazor's server-side and client-side features. For more detailed instructions on setting up your project, including choosing render modes and configuring for client or server-side rendering, you might want to consult the official Microsoft documentation

## Rendering Modes
The Blazor Web App template in Visual Studio 2022 notably includes three rendering modes for applications: Server, WebAssembly (Client), and Automatic. The Server mode executes application logic on the server, utilizing SignalR for dynamic UI updates. The WebAssembly mode allows the application to run directly in the browser, leveraging .NET on WebAssembly for client-side execution. The innovative Automatic mode intelligently combines both, starting with server-side rendering for a quick initial load and transitioning to WebAssembly for rich interactivity, optimizing both performance and the user experience. For more detailed insights, you're encouraged to refer to the official Microsoft documentation on Blazor project templates and rendering modes.

## Localization in Blazor Applications
Localization in Blazor applications refers to the process of adapting your application to support multiple languages and cultures. This is crucial for developing global applications that cater to a diverse user base.

### Microsoft.Extensions.Localization Middleware
* The Microsoft.Extensions.Localization middleware is a key component for enabling localization in both server and client-side Blazor applications. It provides services and interfaces for retrieving localized strings from resource files, allowing developers to create multilingual applications efficiently.

### Server-Side Localization
On the server side, localization involves configuring the middleware in the Startup.cs or Program.cs file and using it to serve localized content based on the user's preferences or settings.

### Client-Side Localization
For client-side Blazor applications, localization can be achieved by integrating the localization library and ensuring that the application loads and applies the appropriate resources based on the user's language settings.

### Organizing Localization Resources
Organizing localization files in a specific folder, separate from the Blazor components and pages, offers several advantages:

* Modularity: Keeps localization resources centralized, making them easier to manage and update.
* Scalability: Facilitates the addition of new languages and cultures without cluttering the application structure.
* Separation of Concerns: Maintains a clear separation between UI components and localization resources, adhering to good software development practices.

* This organization strategy enhances the maintainability and scalability of Blazor applications, making it simpler to support multiple languages and cultures as the application grows.

By adopting these practices and leveraging Blazor Web App 8's features, developers can create robust, modern web applications that are accessible to a global audience.

## how to use our template
To implement localization in your code using our repository, fork or download our specific functional branch named feature/localization. Follow the provided to-do list, as each step of the code implementation is detailed there. This structured approach ensures a thorough understanding and integration of localization features into your Blazor application.

### todo-list
#### server side
1. create on server side project a folder named Locales
2. create on the server side project a resource file named AppResource.resx and generate it code
3. add all your resources keys
4. add one resources file for each culture 
5. Reference Microsoft.Extensions.Localizations package

#### client side
1. create on client side project a folder named Locales
2. create on the client side project for each client component and page blazor named.resx and generate it coe
3. add all your resources keys
4. add one resource file for each component and page and each culture
5. Reference Microsoft.Extensions.Localizations package

#### following the code todo-list
1. display todolist
2. view or change code include to adapt to your project 

## Authentication
This module focuses on implementing server-side authentication using *Microsoft.AspNetCore.Identity.EntityFrameworkCore*. 
We'll explore creating a login form that utilizes email and password for user authentication. 
A significant portion will be dedicated to implementing two-factor authentication, enhancing the security of our application.

On the client-side, we'll delve into Microsoft.AspNetCore.Components.WebAssembly.Authentication, discussing how to implement AuthenticationStateProvider and a redirection component for handling unauthenticated users or expired sessions.

Additionally, we'll cover the integration of Single Sign-On (SSO) with social media accounts like Facebook and Google, providing a streamlined login experience.

Finally, an important aspect for enterprises, the integration with Microsoft Identity Platform for authentication using Azure AD, will be discussed, offering a robust solution for managing user identities and permissions.

This README outlines the key components and steps involved in securing your application, providing a solid foundation for both server and client-side authentication strategies.

### implement User Flow with login and Password
[Read this readme](./README.Authentication.md)

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

