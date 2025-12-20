export default function StudentsSearchBar({search, onSearchChange, onSearch}) {
    return(
        <div className="input-group">
            <input
                className="form-control"
                value={search}
                onChange={(e) => onSearchChange(e.target.value)}
                placeholder="Search by name/email..."
                onKeyDown={(e) => {if(e.key === 'Enter') onSearch()}}
            />
        </div>
    );
}