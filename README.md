[![.NET Worker](https://github.com/RamblingGeekUK/BoW/actions/workflows/dotnet-worker.yml/badge.svg?branch=main)](https://github.com/RamblingGeekUK/BoW/actions/workflows/dotnet-worker.yml)

# BankOfWayne Bot - BOW

On the discord server people will post things to buy, etc.. and I like my gadgets and for fun I started replying with Bank Manager Of Wayne Says this purchase is not allowed, etc, this BOT replies automatically for me. 

It used to use text files to store the the words that would trigger a response, eg Buy, and a text file for the the reponse. I have swapped out the text files for a Supabase. 

For it to fire, I have to be mentioned and so does the trigger phrase for the bot to reply.

# Discord Token

The app will look for an Enviroment varibles named 

```
DiscordBow_TOKEN  // erm... discord API/BOT Token
SB_BoW_URL		  // Supabase DB URL
SB_BoW_API_Key	  // Supabase API 
```

To set on Linux : 

```
export DiscordBow_TOKEN=<key>
SB_BoW_URL=<url>
SB_BoW_API_Key=<key>
```

To Set on Windows (via PowerShell): 

```
$env:DiscordBow = '<key>'
$env:SB_BoW_URL = '<url>'
$env:SB_BoW_API_Key = '<key>'
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

For 32bit (Raspberry OS)

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
