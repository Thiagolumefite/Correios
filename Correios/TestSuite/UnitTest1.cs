using NUnit.Framework;
using Correios.AssertObject;
using Correios.PageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace Correios
{
    public class Tests
    {
        IWebDriver driver;
        HomePO HomePO;
        TudoSobreCepPO TudoSobreCepPO;
        BuscaCepPO BuscaCepPO;
        BuscaCepAsserts BuscaCepAsserts;
        RastreioObjetoPO RastreioObjetoPO;
        RastreioObjetoAsserts RastreioObjetoAsserts;

        [SetUp]
        public void Setup()
        {

            //var options = new ChromeOptions()
            //{
            //    AcceptInsecureCertificates = true,
            //};
            //options.AddArgument("ignore-certificate-errors");
            //options.AddArguments("--lang=pt-BR");
            //options.AddArgument("disable-extensions");
            //options.AddArgument("--no-sandbox");
            //options.AddArguments("--incognito");
            //options.AddExcludedArgument("enable-automation");
            //driver = new ChromeDriver(options);

            driver = new ChromeDriver();

            HomePO = new HomePO(driver);
            TudoSobreCepPO = new TudoSobreCepPO(driver);
            BuscaCepPO = new BuscaCepPO(driver);
            BuscaCepAsserts = new BuscaCepAsserts(driver);
            RastreioObjetoPO = new RastreioObjetoPO(driver);
            RastreioObjetoAsserts = new RastreioObjetoAsserts(driver);


            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(1000);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.correios.com.br/");

            driver.FindElement(By.Id("btnCookie")).Click();
            //Thread.Sleep(1000);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void BuscarCepEndInvalido()
        {
            HomePO.AcessarTudoSobreCEP();
            TudoSobreCepPO.AcessarBuscadorCEP();

            driver.SwitchTo().Window(driver.WindowHandles[1]);
            BuscaCepPO.BuscarCepOuEnd("80700-000");
            BuscaCepAsserts.BuscarCepEndInvalido();
        }

        [Test]
        public void BuscarCepEndValido()
        {
            HomePO.AcessarTudoSobreCEP();
            TudoSobreCepPO.AcessarBuscadorCEP();

            driver.SwitchTo().Window(driver.WindowHandles[1]);
            BuscaCepPO.BuscarCepOuEnd("01013-000");
            BuscaCepAsserts.BuscarCepEndValido("Rua Quinze de Novembro", "São Paulo/SP");
        }

        [Test]
        public void BuscarCodRastreio()
        {
            HomePO.PesquisarObjeto("SS987654321BR");
            driver.SwitchTo().Window(driver.WindowHandles[1]);

            RastreioObjetoPO.Pesquisar();
            RastreioObjetoAsserts.CaptchaCodInvalido();

            //TODO resolver manualmente ou burlar Captcha
            RastreioObjetoPO.Pesquisar();

            var m = RastreioObjetoPO.ResultadoMsg();
            RastreioObjetoAsserts.BuscarCodInvalido();

        }

        [Test]
        public void E2E()
        {
            HomePO.AcessarTudoSobreCEP();
            TudoSobreCepPO.AcessarBuscadorCEP();

            driver.SwitchTo().Window(driver.WindowHandles[1]);
            BuscaCepPO.BuscarCepOuEnd("80700-000");
            BuscaCepAsserts.BuscarCepEndInvalido();

            BuscaCepPO.NovaPesquisa();

            BuscaCepPO.BuscarCepOuEnd("01013-000");
            BuscaCepAsserts.BuscarCepEndValido("Rua Quinze de Novembro", "São Paulo/SP");

            BuscaCepPO.RetornaHome();
        }

        [Test]
        public void E2E_comRastreio()
        {
            HomePO.AcessarTudoSobreCEP();
            TudoSobreCepPO.AcessarBuscadorCEP();

            driver.SwitchTo().Window(driver.WindowHandles[1]);
            BuscaCepPO.BuscarCepOuEnd("80700-000");
            BuscaCepAsserts.BuscarCepEndInvalido();

            BuscaCepPO.NovaPesquisa();

            BuscaCepPO.BuscarCepOuEnd("01013-000");
            BuscaCepAsserts.BuscarCepEndValido("Rua Quinze de Novembro", "São Paulo/SP");

            BuscaCepPO.RetornaHome();
            //Thread.Sleep(1000); 

            HomePO.PesquisarObjeto("SS987654321BR");
            driver.SwitchTo().Window(driver.WindowHandles[2]);

            RastreioObjetoPO.Pesquisar();
            RastreioObjetoAsserts.CaptchaCodInvalido();

            //TODO resolver manualmente ou burlar Captcha
            RastreioObjetoPO.Pesquisar();

            var m = RastreioObjetoPO.ResultadoMsg();
            RastreioObjetoAsserts.BuscarCodInvalido();
        }

    }
}