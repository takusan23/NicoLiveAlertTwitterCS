using System.ComponentModel;

public class AccountListViewData : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public string ID { get; set; }
    public string Name { get; set; }
    public int Pos { get; set; }

}