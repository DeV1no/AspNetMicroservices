﻿using System;

namespace Ordering.Application.Exceptions;

public partial class NotFoundException:ApplicationException
{
    public NotFoundException(string name, object key)
        :base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}