﻿namespace sdu.bachelor.microservice.catalog.Entities;

public class Reservation
{
    public Guid CustomerId { get; set; }
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }

}
