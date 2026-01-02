export default function PaginationBar({
    page, 
    totalPages, 
    onPageChange}) {
    
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
                page {page} of {totalPages}
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