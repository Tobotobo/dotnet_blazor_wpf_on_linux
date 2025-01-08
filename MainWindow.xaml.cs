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

        // // 埋め込みリソースから `wwwroot` を提供
        // var embeddedProvider = new ManifestEmbeddedFileProvider(
        //     Assembly.GetExecutingAssembly(), 
        //     "wwwroot");


        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();
        serviceCollection.AddBlazorWebViewDeveloperTools(); // 開発者ツールを有効にする ※F12
        // serviceCollection.AddSingleton<IFileProvider>(embeddedProvider);

        Resources.Add("services", serviceCollection.BuildServiceProvider());
    }
}