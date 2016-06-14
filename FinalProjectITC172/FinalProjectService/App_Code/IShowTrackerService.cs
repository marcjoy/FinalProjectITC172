using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IShowTrackerService" in both code and config file together.
[ServiceContract]
public interface IShowTrackerService
{
    [OperationContract]
    List<string> GetShows();

    [OperationContract]
    List<string> GetVenues();

    [OperationContract]
    List<string> GetArtists();

    [OperationContract]

    List<ShowInfo> GetShowByVenue(string VenueName);

    [OperationContract]
    List<ShowInfo> GetShowByArtist(string ArtistName);

    [OperationContract]
    List<ShowInfo> GetShowsForFanArtists(int fanKey);

    [OperationContract]
    bool RegisterFan(Fan f, FanLogin fl);

    [OperationContract]
    int FanLogin(string fanUsername, string fanPassword);

    [OperationContract]
    int AddFanArtist(int fanKey, string artistName);

    
}

[DataContract]
public class ShowInfo
{
    [DataMember]
    public string ShowName { set; get; }

    [DataMember]
    public string ShowDate { set; get; }

    [DataMember]
    public string ShowTime { set; get; }

    [DataMember]
    public string VenueName { set; get; }

    [DataMember]
    public string TicketInfo { set; get; }

    [DataMember]
    public string ArtistName { set; get; }

}
