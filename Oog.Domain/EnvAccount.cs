﻿namespace Oog.Domain;

public class EnvAccount
{
    public required int EnvId { get; set; }
    public required int AccountId { get; set; }
    public required bool Owner {  get; set; }
}