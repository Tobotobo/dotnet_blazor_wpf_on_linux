using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace dotnet_blazor_wpf_on_linux;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();
        serviceCollection.AddBlazorWebViewDeveloperTools(); // 開発者ツールを有効にする ※F12

        // カスタム BlazorWebView をウインドウのメインコンテンツに設定
        CustomBlazorWebView customBlazorWebView = new() {
            HostPage="wwwroot/index.html",
            Services=serviceCollection.BuildServiceProvider(),
        };
        customBlazorWebView.RootComponents.Add(new RootComponent
        {
            Selector = "#app",
            ComponentType = typeof(Counter)
        });
        this.Content = customBlazorWebView;
    }

    // 埋め込みリソースから wwwroot を取得するカスタム BlazorWebView
    private class CustomBlazorWebView : BlazorWebView {
        public override IFileProvider CreateFileProvider(string _)
        {
            // 埋め込みリソースから wwwroot を提供
            EmbeddedFileProvider embeddedFileProvider = new EmbeddedFileProvider(
                Assembly.GetExecutingAssembly(),
                $"{typeof(App).Namespace}.wwwroot");
            return embeddedFileProvider;
        }
    }
}