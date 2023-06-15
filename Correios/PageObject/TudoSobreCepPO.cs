using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Correios.PageObject
{
    public class TudoSobreCepPO
    {
        private readonly IWebDriver WebDriver;
        private TimeSpan TimeOut = TimeSpan.FromSeconds(60);
        private WebDriverWait Wait => new WebDriverWait(WebDriver, TimeOut);

        public TudoSobreCepPO(IWebDriver webDriver) => WebDriver = webDriver;

        //Mapeamento dos elementos da Página
        protected IWebElement LinkCepPage => Wait.Until(d => d.FindElement(By.LinkText("Acesse e consulte o CEP correto para realizar um envio.")));

        public void AcessarBuscadorCEP() => LinkCepPage.Click();

    }
}
