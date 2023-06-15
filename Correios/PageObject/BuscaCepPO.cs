using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Correios.Componentes;
using System.Collections.Specialized;
using System.Threading;

namespace Correios.PageObject
{
    public class BuscaCepPO
    {
        //declarações
        private readonly IWebDriver WebDriver;
        private TimeSpan TimeOut = TimeSpan.FromSeconds(60);
        private WebDriverWait Wait => new WebDriverWait(WebDriver, TimeOut);
        public BuscaCepPO(IWebDriver webDriver) => WebDriver = webDriver;

        //Mapeamento dos elementos da Página

        private IWebElement btnLogo => Wait.Until(d => d.FindElement(By.ClassName("logo")));

        private IWebElement End => Wait.Until(d => d.FindElement(By.Id("endereco")));
        private IWebElement BtnBusca => Wait.Until(d => d.FindElement(By.XPath("/html/body/main/form/div[1]/div[1]/div/section/div[3]/div/div/button")));
        private IWebElement Msg => Wait.Until(d => d.FindElement(By.Id("mensagem-resultado")));
        private IWebElement BtnNBusca => Wait.Until(d => d.FindElement(By.XPath("//*[@id=\"btn_nbusca\"]")));
        private IWebElement ResultadoDNEC => Wait.Until(d => d.FindElement(By.Id("resultado-DNEC")));
        private Table TblResultado => new Table(ResultadoDNEC);

        // Métodos de manipulação 
        public void RetornaHome() => btnLogo.Click();
        public void Pesquisar() => BtnBusca.Click();
        public void NovaPesquisa() => BtnNBusca.Click();
        public void PreencherCEP(string end) => End.SendKeys(end);
        public string ResultadoMsg() => Msg.Text;


        public void BuscarCepOuEnd(string end)
        {
            PreencherCEP(end);
            Pesquisar();
            Thread.Sleep(500);
        }

        public bool? EhEndCepValido()
        {
            if (ResultadoMsg().Equals("Resultado da Busca por Endereço ou CEP", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (ResultadoMsg().Equals("Não há dados a serem exibidos", StringComparison.OrdinalIgnoreCase))
                return false;
            else
                return null;
        }


        public (string End, string Bairro, string CidUF, string Cep) RetornaLogradouro()
        {
            var columnNames = TblResultado.ColumnNames;

            var idxLogradouro = columnNames.IndexOf("Logradouro/Nome");
            var idxBairro = columnNames.IndexOf("Bairro/Distrito");
            var idxCidadeUF = columnNames.IndexOf("Localidade/UF");
            var idxCep = columnNames.IndexOf("CEP");

            var row = TblResultado.GetRow(1);

            var end = row.GetCell(idxLogradouro).Text;
            var bairro = row.GetCell(idxBairro).Text;
            var cidadeUF = row.GetCell(idxCidadeUF).Text;
            var cep = row.GetCell(idxCep).Text;

            return (end,bairro,cidadeUF,cep);
        }
    }
}