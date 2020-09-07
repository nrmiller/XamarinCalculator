using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinCalculator.Services;
using XamarinCalculator.ViewModels;
using XamarinCalculator.Views;

namespace XamarinCalculator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = new CalculatorPage()
            {
                BindingContext = new CalculatorViewModel(new EditorService())
            };

            MainPage = new NavigationPage(page);
        }
    }
}
