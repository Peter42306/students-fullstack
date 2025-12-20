import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getStudent } from "../../api/studentsApi";

export default function StudentDetails() {

    const {id} = useParams();
    const [student, setStudent] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        (async () => {
            setLoading(true);
            setError('');

            try {
                setStudent(await getStudent(id));
            } catch (err) {
                setError(err?.message ?? 'Load failed');
            } finally {
                setLoading(false);
            }
        })();
    }, [id]);

    if(loading){
        return (
            <div className="container py-3">
                Loading...
                {/* <div className="alert alert-info">
                    Loading...
                </div> */}
            </div>
        ) 
    } 

    if(error){
        return(
            <div className="container py-3">        
                <div className="alert alert-danger">
                    {error}
                </div>
            </div>
        )   
    }

    if(!student){
        return null;
    }


    

    return(
        <div className="container py-3">
            <div className="d-flex justify-content-between align-content-center">
                <h1 className="mb-0">Students</h1>                
                
                <div className="text-end">
                    <Link 
                        to="/students" 
                        className="btn btn-outline-primary btn-fixed"                    
                    >
                        Back to list
                    </Link>
                </div>                            
            </div>
            <hr />                                   

            <h3>{student.firstName} {student.lastName}</h3>

            <div className="d-flex flex-column gap-2">
                <div>Email: {student.email}</div>
                <div>Birth date: {student.dateOfBirth}</div>
                <div>Age: {student.age}</div>
                <div>Enrollment date: {student.enrollmentDate}</div>
                <div>Study year: {student.yearOfStudy}</div>
                <div>Notes: {student.notes}</div>
            </div>
            

            

            <hr/>
            <div className="text-end">
                <Link 
                    to={`/students/${id}/edit`}
                    className="btn btn-outline-primary btn-fixed"
                >
                    Edit
                </Link>
            </div>
            

            

            
            
        </div>
    );
}