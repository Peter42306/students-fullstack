import { Link } from "react-router-dom";

export default function StudentTable({students, onDelete}) {
    return(
        <div className="table-responsive">
            <table className="table table-hover align-middle">
                <thead>
                    <tr>
                        <th className="text-nowrap">Name</th>
                        <th className="text-nowrap">Email</th>
                        <th className="text-nowrap">Birth Date</th>
                        <th className="text-nowrap">Age</th>
                        <th className="text-nowrap">Study Year</th>
                        <th className="text-nowrap">Notes</th>
                        <th className="text-nowrap">Actions</th>
                    </tr>                    
                </thead>
                <tbody>
                    {students.map((s) => (
                        <tr>
                            <td className="text-nowrap">{s.firstName} {s.lastName}</td>
                            <td className="text-nowrap">{s.email}</td>
                            <td className="text-nowrap">{s.dateOfBirth}</td>
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