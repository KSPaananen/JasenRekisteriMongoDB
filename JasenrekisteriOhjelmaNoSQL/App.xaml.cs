﻿namespace JasenrekisteriOhjelmaNoSQL
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = 800;
            window.Height = 675;

            return window;
        }
    }
}
