export default function PaginationBarRange({
    page, 
    pageSize, 
    totalCount, 
    totalPages, 
    onPageChange}) {
    
    const from = totalCount === 0 
        ? 0 
        : (page - 1) * pageSize + 1;

    const to = Math.min(page * pageSize, totalCount);
    
    const canPrev = page > 1;
    const canNext = page < totalPages;

    return(
        <div className="btn-group">
            <button
                className="btn btn-outline-secondary btn-sm"
                disabled={!canPrev}
                onClick={() => onPageChange(1)}
            >
                &lt;&lt;
            </button>
            <button
                className="btn btn-outline-secondary btn-sm"
                disabled={!canPrev}
                onClick={() => onPageChange(page - 1)}
            >
                &lt;
            </button>
            <button
                className="btn btn-outline-secondary btn-sm text-nowrap"                
                disabled                
            >
                {from}-{to} of {totalCount}
            </button>
            <button
                className="btn btn-outline-secondary btn-sm"
                disabled={!canNext}
                onClick={() => onPageChange(page + 1)}
            >
                &gt;&gt;
            </button>
            <button
                className="btn btn-outline-secondary btn-sm"
                disabled={!canNext}
                onClick={() => onPageChange(totalPages)}
            >
                &gt;
            </button>
        </div>
    );
}