using System;
using System.Xml.Serialization;

namespace MemoryGame.Models
{
    [Serializable]
    public class User
    {
        public string Username { get; set; }
        public string ImagePath { get; set; }
    }
}