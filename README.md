# dotnet_blazor_wpf_on_linux

## 概要

Windows Presentation Foundation (WPF) の Blazor アプリを構築する ※.NET 8  
https://learn.microsoft.com/ja-jp/aspnet/core/blazor/hybrid/tutorials/wpf?view=aspnetcore-8.0  

## 環境
* Docker version 26.1.3, build b72abbb
* mcr.microsoft.com/dotnet/sdk:8.0
* dotnet version 8.0.404
* wpf template version 8.0.11

## 詳細

※ Linux 環境に WPF のテンプレートをインストールする手順については

### WPF プロジェクトの作成

```sh
dotnet new wpf
```

プロジェクトファイル( dotnet_blazor_wpf_on_linux.csproj ) の PropertyGroup に以下を追加する。
```xml
<EnableWindowsTargeting>true</EnableWindowsTargeting>
```

```sh
dotnet restore
```

### Blazor を組み込む

Microsoft.AspNetCore.Components.WebView.Wpf パッケージをプロジェクトに追加

https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebView.Wpf
```sh
dotnet add package Microsoft.AspNetCore.Components.WebView.Wpf --version 8.0.100
```
※9.0 以降では Microsoft.Extensions.DependencyInjection が含まれていないのか参照エラーになる。  
　また、BlazorWebView も存在しないエラーになる。要確認  

プロジェクトファイル( dotnet_blazor_wpf_on_linux.csproj ) の Sdk を以下に変更する。
```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
```

プロジェクトファイル( dotnet_blazor_wpf_on_linux.csproj ) の PropertyGroup に以下を追加する。
```xml
<RootNamespace>dotnet_blazor_wpf_on_linux</RootNamespace>
```

_Imports.razor を作成する
```razor
@using Microsoft.AspNetCore.Components.Web
```

wwwroot フォルダを作成する
```
mkdir wwwroot
```

wwwroot/index.html を作成する。  
留意点
* html lang を ja に変更 ※ブラウザで開いた時に、いちいち翻訳するか表示されるため
* `<link href="XXXXXX.styles.css" rel="stylesheet" />` の XXXXXX は先ほどの `RootNamespace` に合わせる
* title はお好みで変更
```html
<!DOCTYPE html>
<html lang="ja">

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>dotnet_blazor_wpf_on_linux</title>
    <base href="/" />
    <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="css/app.css" rel="stylesheet" />
    <link href="dotnet_blazor_wpf_on_linux.styles.css" rel="stylesheet" />
</head>

<body>
    <div id="app">Loading...</div>

    <div id="blazor-error-ui" data-nosnippet>
        An unhandled error has occurred.
        <a href="" class="reload">Reload</a>
        <a class="dismiss">🗙</a>
    </div>
    <script src="_framework/blazor.webview.js"></script>
</body>

</html>
```

wwwroot/css フォルダを作成する。
```
mkdir wwwroot/css
```

wwwroot/css/app.css を作成する。
```css
html, body {
    font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
}

h1:focus {
    outline: none;
}

a, .btn-link {
    color: #0071c1;
}

.btn-primary {
    color: #fff;
    background-color: #1b6ec2;
    border-color: #1861ac;
}

.valid.modified:not([type=checkbox]) {
    outline: 1px solid #26b050;
}

.invalid {
    outline: 1px solid red;
}

.validation-message {
    color: red;
}

#blazor-error-ui {
    background: lightyellow;
    bottom: 0;
    box-shadow: 0 -1px 2px rgba(0, 0, 0, 0.2);
    display: none;
    left: 0;
    padding: 0.6rem 1.25rem 0.7rem 1.25rem;
    position: fixed;
    width: 100%;
    z-index: 1000;
}

    #blazor-error-ui .dismiss {
        cursor: pointer;
        position: absolute;
        right: 0.75rem;
        top: 0.5rem;
    }
```

wwwroot/css/bootstrap.min.css を格納する ※省略

Counter.razor を作成する
```html
<h1>カウンター</h1>

<p>現在のカウント: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">加算</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

MainWindow.xaml 
```xml
xmlns:blazor="clr-namespace:Microsoft.AspNetCore.Components.WebView.Wpf;assembly=Microsoft.AspNetCore.Components.WebView.Wpf"
```
```xml
        <blazor:BlazorWebView HostPage="wwwroot\index.html" Services="{DynamicResource services}">
            <blazor:BlazorWebView.RootComponents>
                <blazor:RootComponent Selector="#app" ComponentType="{x:Type local:Counter}" />
            </blazor:BlazorWebView.RootComponents>
        </blazor:BlazorWebView>
```

MainWindow.xaml.cs
```cs
using Microsoft.Extensions.DependencyInjection;
```

```cs
var serviceCollection = new ServiceCollection();
serviceCollection.AddWpfBlazorWebView();
Resources.Add("services", serviceCollection.BuildServiceProvider());
```

```sh
dotnet publish
```