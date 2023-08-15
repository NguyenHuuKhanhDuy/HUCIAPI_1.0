using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Import
{
    public Guid Id { get; set; }

    public string ImportNumber { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int Total { get; set; }

    public bool IsDelete { get; set; }

    public int StatusImportId { get; set; }

    public string StatusImportName { get; set; } = null!;

    public Guid UserCreateId { get; set; }

    public Guid SupplierId { get; set; }

    public virtual ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();

    public virtual StatusImport StatusImport { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Employee UserCreate { get; set; } = null!;
}
