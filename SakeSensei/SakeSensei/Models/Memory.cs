using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SakeSensei.Models
{
    public class Memory
    {
        String _image;
        DateTime _dateRecorded;
        String _sakeName;
        String _brewery;
        String _classification;
        String _rating;
        String _notes;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
            }
        }

        public DateTime DateRecorded
        {
            get
            {
                return _dateRecorded;
            }

            set
            {
                _dateRecorded = value;
            }
        }

        public string SakeName
        {
            get
            {
                return _sakeName;
            }

            set
            {
                _sakeName = value;
            }
        }

        public string Brewery
        {
            get
            {
                return _brewery;
            }

            set
            {
                _brewery = value;
            }
        }

        public string Classification
        {
            get
            {
                return _classification;
            }

            set
            {
                _classification = value;
            }
        }

        public string Rating
        {
            get
            {
                return _rating;
            }

            set
            {
                _rating = value;
            }
        }

        public string Notes
        {
            get
            {
                return _notes;
            }

            set
            {
                _notes = value;
            }
        }

        public Memory()
        {

        }

        public Memory(string sakeName)
        {
            SakeName = sakeName;
        }

        public bool SetProp(string propertyName, string data)
        {
            try
            {
                PropertyInfo prop = this.GetType().GetProperty(propertyName);
                if (prop != null && prop.CanWrite)
                {
                    if (prop.GetType() == Type.GetType("String")) prop.SetValue(this, data, null);
                    if (prop.GetType() == Type.GetType("DateTime")) prop.SetValue(this, Convert.ToDateTime(data), null);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
