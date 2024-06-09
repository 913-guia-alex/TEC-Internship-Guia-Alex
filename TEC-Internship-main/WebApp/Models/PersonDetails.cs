using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;

public class PersonDetail
{
    public int Id { get; set; }
    public DateTime BirthDay { get; set; }
    public string PersonCity { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
}