[![.NET Worker](https://github.com/RamblingGeekUK/BoW/actions/workflows/dotnet-worker.yml/badge.svg?branch=main)](https://github.com/RamblingGeekUK/BoW/actions/workflows/dotnet-worker.yml)

# BankOfWayne Bot - BOW

On the discord server people will post things to buy, etc.. and I like my gadgets, so I started replying with Bank Manager Of Wayne Says not allowed, etc, so I created a bot to automatically reply for me. 

There are two text files, phrases.txt and responses.txt. The phrases file containes all the words that the bot will respond to. The responses is all the possiable responses, one is selected at random. 

I have to be mentioned and so does the watchPhrase for the bot to reply.

# Discord Token

The app will look for an Enviroment varible named 

```
DiscordBow_TOKEN
```

To set on Linux : 

```
export DiscordBow_TOKEN=<key>
```

To Set on Windows (via PowerShell): 

```
$env:DiscordBow = '<key>'
```

# Running on a Pi

Install .dotnet 

```
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel STS
```

```
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc
```

For 32bit (Raspbbery OS)

```
dotnet publish --runtime linux-arm --self-contained
```

For 64Bit (Raspberry OS)

```
dotnet publish --runtime linux-arm64 --self-contained
```

Clone Repo

```
https://github.com/RamblingGeekUK/BoW.git
```
