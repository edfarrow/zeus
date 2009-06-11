﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Isis.ComponentModel;

namespace Zeus.Persistence
{
	public interface IFinder<T> : IService
	{
		IQueryable<T> Items();

		IQueryable<TResult> Items<TResult>()
			where TResult : T;

		IQueryable Items(Type resultType);
	}
}