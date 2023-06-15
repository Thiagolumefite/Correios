using Correios.PageObject;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Correios.AssertObject
{
    public class RastreioObjetoAsserts
    {

        private IWebDriver WebDriver;

        public RastreioObjetoAsserts(IWebDriver webDriver) => WebDriver = webDriver;
        private RastreioObjetoPO RastreioObjetoPO => new RastreioObjetoPO(WebDriver);
        public void BuscarCodInvalido()
        {
            var msg = RastreioObjetoPO.ResultadoMsg();
            StringAssert.AreEqualIgnoringCase("Objeto não encontrado na base de dados dos Correios.", msg);
        }

        public void CaptchaCodInvalido()
        {
            var msg = RastreioObjetoPO.CaptchaMsg();
            StringAssert.AreEqualIgnoringCase("Preencha o campo captcha", msg);
        }


    }
}
