using System;
using System.IO;
using System.Xml;

namespace Mia_Record
{
    class Commands
    {
        public SStruct[] CommandList;
        public int CommandsCount = 0;

        public Commands()
        {
            try
            {
                XmlDocument XMLList = new XmlDocument();
                XMLList.Load("Commands.xml");
                XmlNode Commands = XMLList.DocumentElement;
                CommandsCount = Commands.ChildNodes.Count;

                int index = 0;
                CommandList = new SStruct[CommandsCount];
                foreach (XmlNode Command in Commands)
                {
                    CommandList[index].Audio_URL = Command.Attributes[0].Value;
                    CommandList[index].Program_URL = Command.Attributes[1].Value;
                    index++;
                }
            }
            catch (Exception)
            {
            }
        }

        public void LoadCommands()
        {
            XmlDocument XMLList = new XmlDocument();
            XMLList.Load("Commands.xml");
            XmlNode Commands = XMLList.DocumentElement;
            CommandsCount = Commands.ChildNodes.Count;
            int index = 0;
            CommandList = new SStruct[CommandsCount];
            foreach (XmlNode Command in Commands)
            {
                CommandList[index].Audio_URL = Command.Attributes[0].Value;
                CommandList[index].Program_URL = Command.Attributes[1].Value;
                index++;
            }
        }

        public void RemoveCommand(int id)
        {
            XmlDocument XMLList = new XmlDocument();
            XMLList.Load("Commands.xml");
            XmlNode Commands = XMLList.DocumentElement;
            Commands.RemoveChild(Commands.ChildNodes[id]);
            XMLList.Save("Commands.xml");

            LoadCommands();
        }

        public void AddCommand(string audio, string program)
        {
            XmlDocument XMLList = new XmlDocument();
            XMLList.Load("Commands.xml");

            XmlNode Command = XMLList.CreateElement("Command");
            XmlAttribute Audio_URL = XMLList.CreateAttribute("Audio_URL");
            XmlAttribute Program_URL = XMLList.CreateAttribute("Program_URL");

            Audio_URL.InnerText = audio;
            Command.Attributes.Append(Audio_URL);

            Program_URL.InnerText = program;
            Command.Attributes.Append(Program_URL);

            XMLList.DocumentElement.AppendChild(Command);

            XMLList.Save("Commands.xml");

            LoadCommands();
        }
    }
}
