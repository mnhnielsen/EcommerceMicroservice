﻿namespace sdu.bachelor.microservice.order.Models;

public class OrderItemDto
{
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Guid ProductId { get; set; }
}

