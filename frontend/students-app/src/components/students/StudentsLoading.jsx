export default function StudentsLoading() {
    return(
        <div className="d-flex align-items-center gap-2 text-secondary mb-2 mt-2">
            <div className="spinner-border text-primary" role="status" aria-hidden="true"/>
            Loading...
        </div>        
    );
}