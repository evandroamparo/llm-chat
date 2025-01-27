# ChatApp

## Overview
ChatApp is a console application that simulates a chat assistant designed to sell an internet plan to a customer. The assistant uses the OpenAI-compatible API to generate responses and is configured to be persistent and persuasive.

It's currently using the free [Groq API](https://groq.com/).

## Prerequisites
- .NET 9.0 SDK or later
- A Groq API key

## Setup

1. **Clone the repository:**
    ```sh
    git clone <repository-url>
    cd ChatApp
    ```

2. **Add User Secrets:**
    The project uses user secrets to store sensitive information like the API key. To add your secrets, run the following commands:
    ```sh
    dotnet user-secrets init
    dotnet user-secrets set "GroqApiKey" "<your-groq-api-key>"
    dotnet user-secrets set "ModelName" "<model-name>"
    dotnet user-secrets set "BaseUrl" "<base-url>"
    ```

You can get an API key from [Groq dev console](https://console.groq.com/keys).

The available models are listed [here](https://console.groq.com/docs/models). I suggest Llama because it has a larger context window.

The OpenAI-compatible base URL can be found [here](https://console.groq.com/docs/openai).

3. **Restore dependencies:**
    ```sh
    dotnet restore
    ```

## Running the Application

1. **Build the project:**
    ```sh
    dotnet build
    ```

2. **Run the project:**
    ```sh
    dotnet run
    ```

## Usage
- The application will start and display a welcome message.
- Type your messages and press Enter to interact with the chat assistant.
- Type `exit` to exit the chat.

## License
This project is licensed under the MIT License.
