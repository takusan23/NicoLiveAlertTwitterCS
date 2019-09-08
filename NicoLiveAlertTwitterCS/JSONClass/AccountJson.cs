using System.ComponentModel;

public class NicoFavListJSON : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public string ID { get; set; }
    public string Name { get; set; }
}