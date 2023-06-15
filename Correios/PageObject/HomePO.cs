using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Correios.PageObject
{
    internal class HomePO
    {

        private readonly IWebDriver WebDriver;
        private TimeSpan TimeOut = TimeSpan.FromSeconds(60);
        private WebDriverWait Wait => new WebDriverWait(WebDriver, TimeOut);

        public HomePO(IWebDriver webDriver) => WebDriver = webDriver;

        //Mapeamento dos elementos da Página
        private IWebElement MenuEnviar => Wait.Until(d => d.FindElement(By.Id("itemMenu_enviar")));
        private IWebElement TudoSobreCEP => Wait.Until(d => d.FindElement(By.LinkText("Tudo sobre CEP")));

        private IWebElement Objetos => Wait.Until(d => d.FindElement(By.Id("objetos")));

        public void AcessarTudoSobreCEP()
        {
            MenuEnviar.Click();
            TudoSobreCEP.Click();
        }

        public void PesquisarObjeto(string cod)
        {
            Objetos.SendKeys(cod);
            Objetos.Submit();
            //Thread.Sleep(1000);
        }

    }
}
