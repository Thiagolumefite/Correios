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
    public class BuscaCepAsserts
    {
        private IWebDriver WebDriver;

        public BuscaCepAsserts(IWebDriver webDriver) => WebDriver = webDriver;

        private BuscaCepPO BuscaPO => new BuscaCepPO(WebDriver);
        
        public void BuscarCepEndValido(string end,string cidadeUF)
        {
            //podemos fazer esta validação de duas formas, sendo a primeira diretamente pelo texto do resultado
            var msg = BuscaPO.ResultadoMsg();

            StringAssert.AreEqualIgnoringCase("Resultado da Busca por Endereço ou CEP", msg);

            //e a segunda pelo Booleano processado pela classe BuscaCepPO que avalia internamente o resultado
            var result = BuscaPO.EhEndCepValido();
            Assert.IsTrue(result);

            StringAssert.AreEqualIgnoringCase("Resultado da Busca por Endereço ou CEP", msg);

            //Valida se endereço do resultado corresponde ao endereço esperado
            var logradouro = BuscaPO.RetornaLogradouro();
            var ehRegistroEsperado = logradouro.End.ToUpper().Contains(end.ToUpper()) && logradouro.CidUF.Equals(cidadeUF);
            Assert.IsTrue(ehRegistroEsperado);

            //ou podemos fazer as mesmas validações separadas
            StringAssert.Contains(end.ToUpper(), logradouro.End.ToUpper());
            StringAssert.AreEqualIgnoringCase(cidadeUF, logradouro.CidUF);
        }

        public void BuscarCepEndInvalido()
        {
            //podemos fazer esta validação de duas formas, sendo a primeira diretamente pelo texto do resultado
            var msg = BuscaPO.ResultadoMsg();

            StringAssert.AreEqualIgnoringCase("Não há dados a serem exibidos", msg);

            //e a segunda pelo Booleano processado pela classe BuscaCepPO que avalia internamente o resultado
            var result = BuscaPO.EhEndCepValido();
            Assert.IsFalse(result);
        }


    }
}
