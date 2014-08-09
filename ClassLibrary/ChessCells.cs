/***************************************************************
 * File: Cells.cs
 * Created By: Syed Ghulam Akbar		Date: 27 June, 2005
 * Description: This class is a collection of the Cell objects
 * and provide methods to add and remove chess cells
 ***************************************************************/

using System;
using System.Collections.Specialized;
using System.Collections;
using System.Xml;

namespace ChessLibrary
{
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
	public class Cells
	{
		Hashtable m_Cells;

		public Cells()
		{
			m_Cells = new Hashtable();	// Make a list of the cells
		}

		// return the unique key for the cell
		private string GetKey(Cell newcell)
		{
			return ""+ newcell.row + newcell.col;
		}

		// return the unique key for the cell
		private string GetKey(int row, int col)
		{
			return ""+ row + col;
		}

		// Add new cell to the collection
		public void Add(Cell newcell)
		{
			m_Cells.Add(GetKey(newcell), newcell);	// Add the new object to the cell collection
		}

		// remove the given cell from the collection
		public void Remove(int row, int col)
		{
			string key=GetKey(row,col);
			if (m_Cells.ContainsKey(key)) // if the item exists in the collection
				m_Cells.Remove(key);	  // remove it
		}

		// remove all the cell objects from the collection
		public void Clear()
		{
			m_Cells.Clear();
		}

		// get the new item by rew and column
		public Cell this[int row, int col]
		{
			get
			{
				return (Cell)m_Cells[GetKey(row,col)];
			}
		}

		// get the new item by string location
		public Cell this[string strloc]
		{
			get
			{
				int col=char.Parse(strloc.Substring(0,1).ToUpper())-64; // Get row from first ascii char i.e. a=1, b=2 and so on
				int row=int.Parse(strloc.Substring(1,1));				  // Get column value directly, as it's already numeric
				return (Cell)m_Cells[GetKey(row,col)];
			}
		}

        /// <summary>
        /// Serialize the Game object as XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public XmlNode XmlSerialize(XmlDocument xmlDoc)
        {
            XmlElement xmlNode = xmlDoc.CreateElement("Cells");

            string xml = "";
            // Serialize and append every cell of this board
            for (int row = 1; row <= 8; row++)
                for (int col = 1; col <= 8; col++)
                {
                    Cell cell = this[row, col];

                    xml += XMLHelper.XmlSerialize(typeof(Cell), cell);
                }

            xmlNode.InnerXml = xml;

            // Return this as String
            return xmlNode;
        }

        /// <summary>
        /// DeSerialize the Side object from XML String
        /// </summary>
        /// <returns>XML containing the Side object state XML</returns>
        public void XmlDeserialize(XmlNode xmlCells)
        {
            // Serialize and append to the side object
            XmlNode cellXml = xmlCells.FirstChild;

            // Serialize and append every cell of this board back to chess cells
            for (int row = 1; row <= 8; row++)
                for (int col = 1; col <= 8; col++)
                {
                    Cell cell = (Cell)XMLHelper.XmlDeserialize(typeof(Cell), cellXml.OuterXml);
                    m_Cells[GetKey(row, col)] = cell;

                    // Get the next node XML
                    cellXml = cellXml.NextSibling;
                }
        }
	}
}
