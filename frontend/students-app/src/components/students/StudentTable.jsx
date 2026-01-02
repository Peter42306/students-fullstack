import { Link } from "react-router-dom";

function SortHeader({
    label, 
    col, 
    sortBy, 
    sortDirection, 
    onSort
}) {
    const sortable = Boolean(col && onSort);
    const active = sortable && sortBy === col;
    let iconClass =  null;
    if(sortable) {
        iconClass = active    
            ? (sortDirection === "desc" ? "bi bi-arrow-up" : "bi bi-arrow-down")
            : "bi bi-arrow-down-up"
    }

    return(
        <th className="text-nowrap">
            
            <div className="d-flex align-items-center gap-1">               
                <span>{label}</span>                    
                    
                <button
                    type="button"                
                    className="btn btn-sm p-0 text-dark"
                    onClick={() => sortable && onSort(col)}                        
                    title="Toggle sort"
                >                                                
                    <span><i className={iconClass} /></span>                                                              
                </button>                                
            </div>                                       
                    
                
            </th>
        );
        
    }

export default function StudentTable({students, onDelete, sortBy, sortDirection, onSort}) {
    
    return(
        <div className="table-responsive">
            <table className="table table-hover align-middle">
                <thead>
                    <tr>
                        <SortHeader label="Name" col="name" sortBy={sortBy} sortDirection={sortDirection} onSort={onSort}/>
                        <SortHeader label="Email" col="email" sortBy={sortBy} sortDirection={sortDirection} onSort={onSort}/>                        
                        <SortHeader label="Age" col="age" sortBy={sortBy} sortDirection={sortDirection} onSort={onSort}/>                        
                        <SortHeader label="Study Year" col="studyYear" sortBy={sortBy} sortDirection={sortDirection} onSort={onSort}/>                        
                        <th className="text-nowrap">Notes</th>
                        <th className="text-nowrap">Actions</th>
                    </tr>                    
                </thead>
                <tbody>
                    {students.map((s) => (
                        <tr key={s.id}>
                            <td className="text-nowrap">{s.lastName} {s.firstName}</td>
                            <td className="text-nowrap">{s.email}</td>
                            
                            <td className="text-nowrap">{s.age}</td>
                            <td className="text-nowrap">{s.yearOfStudy}</td>
                            <td className="text-nowrap text-truncate" style={{maxWidth:"200px"}}>{s.notes}</td>
                            <td>
                                <div className="d-flex justify-content-end gap-2">
                                    <Link
                                        to={`/students/${s.id}`}
                                        className="btn btn-outline-secondary btn-fixed"
                                    >
                                        Details                                    
                                    </Link>                                    
                                    <button
                                        className="btn btn-outline-danger btn-fixed"
                                        onClick={() => onDelete(s.id)}
                                    >
                                        Delete
                                    </button>                                    
                                </div>                                
                            </td>
                        </tr>
                    ))}                    
                </tbody>
            </table>
        </div>
    );
}