/***************************************************************
 * File: Side.cs
 * Created By: Syed Ghulam Akbar		Date: 27 June, 2005
 * Description: A class for the chess side i.e. White or Black.
 * 
 ***************************************************************/

using System;
using System.Xml;

namespace ChessLibrary
{
	/// <summary>
	/// Summary description for Side.
	/// </summary>
    [Serializable]
	public class Side
	{
        SideType m_side;
		public enum SideType {White, Black};

        // Initialize a side with given type
        public Side()
        {
        }

		// Initialize a side with given type
        public Side(SideType side)
		{
			m_side=side;
		}

		// Set the side type
        public SideType type
		{
			get
			{
				return m_side;
			}
			set
			{
				m_side = value;
			}
		}

		// Return true if the side is white
		public bool isWhite()
		{
            return (this.type == SideType.White);
		}

		// Return true if the side is black
		public bool isBlack()
		{
            return (this.type == SideType.Black);
		}

		// Returns the enemy type
        public Side.SideType Enemy()
		{
            if (this.type == SideType.White)
                return SideType.Black;
			else
                return SideType.White;
		}

		// return true if the other side is of enemy
		public bool isEnemy(Side other)
		{
			return (this.type != other.type);
		}

        /// <summary>
        /// Serialize the Game object as XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public XmlNode XmlSerialize(XmlDocument xmlDoc)
        {
            XmlElement xmlNode = xmlDoc.CreateElement("Side");

            // Serialize and append to the side object
            xmlNode.InnerXml = XMLHelper.XmlSerialize(typeof(SideType), m_side);

            // Return this as String
            return xmlNode;
        }

        /// <summary>
        /// DeSerialize the Side object from XML String
        /// </summary>
        /// <returns>XML containing the Side object state XML</returns>
        public void XmlDeserialize(XmlNode xmlSide)
        {
            // Serialize and append to the side object
            m_side = (SideType) XMLHelper.XmlDeserialize(typeof(SideType), xmlSide.InnerXml);
        }
	}
}
