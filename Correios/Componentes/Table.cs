using Correios.Extensoes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Correios.Componentes
{
    public class Table : IWebElement
    {
        private readonly IWebElement _webElement;

        private bool isTable;
        public List<string> ColumnNames => GetHeader().GetColumnNames() ?? new List<string>();
        public List<TableRow> Rows => GetRows();

        public Table(IWebElement webElement)
        {
            this._webElement = webElement ?? throw new ArgumentNullException("element", "element cannot be null");

            isTable = _webElement.TagName.ToLower() == "table";
            if (!isTable)
                throw new UnexpectedTagNameException("table", webElement.TagName);
        }

        private List<TableRow> GetRows()
        {
            List<TableRow> rows = new List<TableRow>();

            var trs = _webElement.FindElements(By.TagName("tr")).ToList();

            int idx = 0;
            foreach (IWebElement tr in trs)
            {
                rows.Add(new TableRow(tr, idx));
                idx++;
            }
            return rows;
        }

        public virtual TableRow GetRow(int index)
        {
            return GetRows()[index];
        }

        /// <summary>
        /// Retorna apenas os WebElements correspondentes a cada campo/coluna.
        /// Não retorna a lista de valores contidos no campo, para isso use GetColumn
        /// </summary>
        /// <returns></returns>
        public virtual TableRow GetHeader()
        {
            var result = Rows.Where(x => x.rowType == TableRow.RowType.HEAD)?.FirstOrDefault() ?? GetRow(0);
            return result;
        }

        /// <summary>
        /// Retorna Lista de valores (registros) contidos em um campo(Coluna) especifico
        /// </summary>
        /// <param name="columnName">Titulo da coluna</param>
        /// <returns></returns>
        //public virtual List<TableCell> GetEntriesByColumn(string columnName)
        public virtual List<TableCell> GetColumn(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException();

            var idxColumn = ColumnNames.IndexOf(columnName);
            return GetColumn(idxColumn);
        }


        /// <summary>
        /// Retorna Lista de valores (registros) contidos em um campo(Coluna) especifico
        /// </summary>
        /// <param name="columnIndex">Indice da coluna</param>
        /// <returns></returns>
        public virtual List<TableCell> GetColumn(int columnIndex)
        {
            if (ColumnNames.Count - 1 < columnIndex)
                throw new IndexOutOfRangeException();

            List<TableCell> cells = new List<TableCell>();
            foreach (var row in Rows)
            {
                cells.Add(row.GetCell(columnIndex));
            }
            return cells;
        }

        public virtual TableCell GetCell(int rowIndex, int columnIndex)
        {
            return GetRow(rowIndex).GetCell(columnIndex);
        }

        public virtual TableCell GetCell(int rowIndex, string columnName)
        {
            var columnIndex = ColumnNames.IndexOf(columnName);
            return GetCell(rowIndex, columnIndex);
        }

        #region implementação de WebElement

        public string TagName => _webElement.TagName;

        public string Text => _webElement.Text;

        public bool Enabled => _webElement.Enabled;

        public bool Selected => _webElement.Selected;

        public Point Location => _webElement.Location;

        public Size Size => _webElement.Size;

        public bool Displayed => _webElement.Displayed;

        public void Clear()
        {
            _webElement.Clear();
        }

        public void SendKeys(string text)
        {
            _webElement.SendKeys(text);
        }

        public void Submit()
        {
            _webElement.Submit();
        }

        public void Click()
        {
            _webElement.Click();
        }

        public string GetAttribute(string attributeName)
        {
            return _webElement.GetAttribute(attributeName);
        }

        public string GetDomAttribute(string attributeName)
        {
            return _webElement.GetDomAttribute(attributeName);
        }

        public string GetDomProperty(string propertyName)
        {
            return _webElement.GetDomProperty(propertyName);
        }

        public ISearchContext GetShadowRoot()
        {
            return _webElement.GetShadowRoot();
        }

        public string GetCssValue(string propertyName)
        {
            return GetCssValue(propertyName);
        }

        public IWebElement FindElement(By by)
        {
            return _webElement.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _webElement.FindElements(by);
        }
        #endregion

    }

    public class TableRow : IWrapsElement, IWebElement
    {
        private readonly IWebElement _webElement;
        private readonly bool isRow;
        public readonly int RowIndex;
        public RowType rowType;

        public enum RowType { HEAD, BODY, FOOT };

        public TableRow(IWebElement webElement, int rowIndex)
        {
            this._webElement = webElement ?? throw new ArgumentNullException("webElement", "webElement cannot be null");

            var parent = _webElement.GetParent();
            var parentName = parent.GetAttribute("tagName");

            //var parent = webElement.GetParentJS();
            //var parentName = parent1.GetAttribute("tagName");

            switch (parentName.ToLower())
            {
                case "tbody":
                    rowType = RowType.BODY;
                    break;
                case "thead":
                    rowType = RowType.HEAD;
                    break;
                case "tfoot":
                    rowType = RowType.FOOT;
                    break;
                default:
                    break;
            }

            isRow = _webElement.TagName.ToLower() == "tr";
            if (!isRow)
            {
                throw new UnexpectedTagNameException("tr", webElement.TagName);
            }
            this.RowIndex = rowIndex;
        }

        public virtual List<TableCell> GetCells()
        {
            List<TableCell> cells = new List<TableCell>();

            List<IWebElement> tds = _webElement.FindElements(By.XPath("th | td")).ToList();

            int idx = 0;
            foreach (IWebElement td in tds)
            {
                cells.Add(new TableCell(td, RowIndex, idx));
                idx++;
            }
            return cells;
        }

        public virtual TableCell GetCell(int columnIndex)
        {
            return GetCells()[columnIndex];
        }

        public List<string> GetColumnNames()
        {
            List<string> columnNames = null;
            if (rowType == RowType.HEAD)
                columnNames = this.GetCells().Select(x => x.Text).ToList();
            return columnNames;
        }

        #region implementação de WebElement
        public string TagName => _webElement.TagName;

        public string Text => _webElement.Text;

        public bool Enabled => _webElement.Enabled;

        public bool Selected => _webElement.Selected;

        public Point Location => _webElement.Location;

        public Size Size => _webElement.Size;

        public bool Displayed => _webElement.Displayed;

        [Obsolete("Usar WebElement")]
        public IWebElement WrappedElement => this._webElement;

        public IWebElement WebElement => this._webElement;

        public void Clear()
        {
            _webElement.Clear();
        }

        public void SendKeys(string text)
        {
            _webElement.SendKeys(text);
        }

        public void Submit()
        {
            _webElement.Submit();
        }

        public void Click()
        {
            _webElement.Click();
        }

        public string GetAttribute(string attributeName)
        {
            return _webElement.GetAttribute(attributeName);
        }

        public string GetDomAttribute(string attributeName)
        {
            return _webElement.GetDomAttribute(attributeName);
        }

        public string GetDomProperty(string propertyName)
        {
            return _webElement.GetDomProperty(propertyName);
        }

        public ISearchContext GetShadowRoot()
        {
            return _webElement.GetShadowRoot();
        }

        public string GetCssValue(string propertyName)
        {
            return GetCssValue(propertyName);
        }

        public IWebElement FindElement(By by)
        {
            return _webElement.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _webElement.FindElements(by);
        }
        #endregion
    }

    public class TableCell : IWebElement
    {
        private readonly IWebElement _webElement;
        private readonly bool isCell;

        public IWebElement WebElement
        {
            get
            {
                return _webElement;
            }
        }

        public readonly int RowIndex;
        public readonly int ColumnIndex;
        public TableCell(IWebElement webElement, int rowIndex, int columnIndex)
        {
            this._webElement = webElement ?? throw new ArgumentNullException("element", "element cannot be null");

            isCell = _webElement.TagName.ToLower() == "td" || _webElement.TagName.ToLower() == "th";
            if (!isCell)
            {
                throw new UnexpectedTagNameException("td / th", webElement.TagName);
            }

            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
        }

        #region implementação de WebElement

        [Obsolete("Usar WebElement")]
        public IWebElement WrappedElement => this._webElement;

        public string TagName => _webElement.TagName;

        public string Text => _webElement.Text;

        public bool Enabled => _webElement.Enabled;

        public bool Selected => _webElement.Selected;

        public Point Location => _webElement.Location;

        public Size Size => _webElement.Size;

        public bool Displayed => _webElement.Displayed;

        public void Clear()
        {
            _webElement.Clear();
        }

        public void SendKeys(string text)
        {
            _webElement.SendKeys(text);
        }

        public void Submit()
        {
            _webElement.Submit();
        }

        public void Click()
        {
            _webElement.Click();
        }

        public string GetAttribute(string attributeName)
        {
            return _webElement.GetAttribute(attributeName);
        }

        public string GetDomAttribute(string attributeName)
        {
            return _webElement.GetDomAttribute(attributeName);
        }

        public string GetDomProperty(string propertyName)
        {
            return _webElement.GetDomProperty(propertyName);
        }

        public ISearchContext GetShadowRoot()
        {
            return _webElement.GetShadowRoot();
        }
        public string GetCssValue(string propertyName)
        {
            return GetCssValue(propertyName);
        }

        public IWebElement FindElement(By by)
        {
            return _webElement.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _webElement.FindElements(by);
        }
        #endregion
    }

}
