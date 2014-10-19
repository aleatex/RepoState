using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace AT
{
	public class RepoState<T>
	{
		#region Constructors

		public RepoState()
		{ Errors = new List<KeyValuePair<string, string>>(); }

		#endregion Constructors

		#region Properties

		public List<KeyValuePair<string, string>> Errors { get; set; }

		public bool IsNotValid
		{
			get { return !IsValid; }
		}

		public bool IsValid
		{
			get { return Errors.Count == 0; }
		}

		public T RepoObj { get; set; }

		#endregion Properties

		#region Exception function

		/// <summary>
		/// Method to add Entity Framework exception msg from DB
		/// </summary>
		/// <param name="e">Entity Framework exception message</param>
		public void AddException(DbEntityValidationException e)
		{
			var errors = e.EntityValidationErrors.AsQueryable()
				.Where(it => !it.IsValid);

			foreach (var error in errors.Select(it => it.ValidationErrors))
				Errors.AddRange(
					error.Select(it => new KeyValuePair<string, string>(it.ErrorMessage, it.PropertyName)));
		}

		/// <summary>
		/// Method to add excetion msg
		/// </summary>
		/// <param name="e">Exception</param>
		public void AddException(Exception e)
		{
			if (e.InnerException != null)
			{
				if (!string.IsNullOrEmpty(e.InnerException.Message))
					Add(e.InnerException.Message);
			}
			if (!string.IsNullOrEmpty(e.Message))
			{
				Add(e.Message);
			}
		}

		#endregion Exception function

		#region Public properties

		public void Error(string error)
		{
			Add(error);
		}

		#endregion Public properties

		#region Private functions

		private void Add(string value)
		{
			Errors.Add(new KeyValuePair<string, string>(string.Format("Error nr {0}", Errors.Count), value));
		}

		#endregion Private functions
	}
}