using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Correios.PageObject
{
    public class RastreioObjetoPO
    {
        private readonly IWebDriver WebDriver;
        private TimeSpan TimeOut = TimeSpan.FromSeconds(60);
        private WebDriverWait Wait => new WebDriverWait(WebDriver, TimeOut);

        public RastreioObjetoPO(IWebDriver webDriver) => WebDriver = webDriver;

        //Mapeamento dos elementos da Página
        private IWebElement Objeto => Wait.Until(d => d.FindElement(By.Id("objeto")));
        private IWebElement Captcha => Wait.Until(d => d.FindElement(By.Id("captcha")));
        private IWebElement MsgAlerta => Wait.Until(d => d.FindElement(By.Id("alerta")).
        FindElement(By.ClassName("msg")));

        private IWebElement BtnPesquisa => Wait.Until(d => d.FindElement(By.ClassName("fa-search")));

        private IWebElement MsgCaptcha => Wait.Until(d => d.FindElement(By.XPath("/html/body/main/div[1]/form/div[2]/div[2]/div[2]/div[3]")));

        public void PreencherObjeto(string cod) => Objeto.SendKeys(cod);
        public void PreencherCaptcha(string cod) => Captcha.SendKeys(cod);
        public void Submit() => Objeto.Submit();
        public void Pesquisar() => BtnPesquisa.Click();

        public string ResultadoMsg()
        {
           Wait.Until<bool>(x=> !MsgAlerta.Text.Equals("Buscando...",StringComparison.OrdinalIgnoreCase));
            return MsgAlerta.Text;
        }

        public string CaptchaMsg() => MsgCaptcha.Text;

    }
}
