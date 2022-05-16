# Simple Chatroom

## Requirements
- Node LTS
- Angular CLI
- Visual Studio 2019/2022
- Docker

## Description
Simple chatroom is a technical backend test project. It consists in two backend servers, one worker process and a client made with Angular 13.
The application is a financial chatroom that allows authorized users to communicate between them and also asks to an automatizated bot for updated quote value of shares.

The bot itself, integrates with a service that provides the info requested and manage to parse the CSV response and send it back to the chatroom.

The communication between the bot and the chatroom is decoupled and made by using RabbitMQ.

The API backend of chatroom consists in an Web REST API made with .NET 5 that also hosts a signalR service to allow clients to react to the changes in the chat. 

An .NET 5 Identity server grant users to connect to the chatroom, serving as authority identity provider for backend API and providing token claims support.

Angular client provides basic route security to access chatroom, obtaining credentials token through authentication with Identity server.

## Application flow

![alt text](https://i.ibb.co/61sT225/simplified.png)

## Basic setup
The project uses In memory databases for simplicity purposes. This databases are seeded for direct testing with two users:
- user: boya password: 123456
- user: nuno password: 654321

...and several test messages to populate the chatroom

Services are all pointing to localhost environment. Assuming that you have installed Docker locally, it's mandatory to have guaranteed connection to a RabbitMQ server, so for that we have to run the following command in the command line:

```sh
docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```
You can add -d parameter to run the server in dettached mode to be able to close the cmd or ps prompt.

After the container with RabbitMQ is running (you can check this through Docker Desktop or docker CLI), you can run the backend solution.

The easiest way is to open the solution in Visual Studio and set multiple startup projects to initiate API, Identity and Robot projects at the same time. You can follow the link below to get guidance about it.

https://docs.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=vs-2022

Once backend solution is running, open the root client project folder and restore the packages running:

```sh
npm install
```

HTTPS is a requirement since we're using OpenId for identity. Running the .NET solution in Visual Studio will prompt you a window to install and trust a developer certificate for local development.

For Angular, we need to generate and trust this certifcate manually using mkcert or some other tool. The link below provides you an example.

https://www.mariokandut.com/how-to-configure-https-ssl-in-angular/

We can now run the project using:

```sh
npm run start
```

The project is running and you can visit

https://www.localhost:4200

Test the project and have fun.
