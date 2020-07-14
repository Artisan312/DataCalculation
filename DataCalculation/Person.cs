using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DataCalculation
{

    public delegate void ChangedHandler(object sender, object v);

    public class Person : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler hander = PropertyChanged;
            if (hander != null)
                hander(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// 属性变换事件
        /// </summary>
        /// <param name="sender">源</param>
        /// <param name="v">变化后的值</param>
        /// <param name="old">变化前的值</param>
        /// <param name="hander"></param>
        protected  virtual void OnChanged(object sender, object v, object old, ChangedHandler hander)
        {
            if (!v.Equals(old) && hander != null)
                hander(sender, v);
        }
    }
}
