import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { getStudent, updateStudent } from "../../api/studentsApi";
import StudentForm from "../../components/students/StudentForm";

export default function StudentUpdate() {

    const {id} = useParams();
    const navigate = useNavigate();

    const [form, setForm] = useState(null);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');

    useEffect(() =>{
        (async () => {
            setLoading(true);
            setError('');

            try {
                const student = await getStudent(id);

                setForm({
                    firstName: student.firstName ?? '',
                    lastName: student.lastName ?? '',
                    email: student.email ?? '',
                    dateOfBirth: (student.dateOfBirth ?? '').slice(0, 10),
                    enrollmentDate: (student.enrollmentDate ?? '').slice(0, 10),
                    notes: student.notes ?? '',
                })
            } catch (err) {
                setError(err?.message ?? 'Load failed')
            } finally {
                setLoading(false);
            }
        })();
    }, [id]);

    async function handleSubmit(e) {
        e.preventDefault();

        if(!form){
            return;
        }

        setSaving(true);
        setError('');

        try {
            await updateStudent(id, form);
            navigate(`/students/${id}`);
        } catch (err) {
            setError(err?.message ?? 'Update failed')
        } finally {
            setSaving(false);
        }
    }

    function handleCancel() {
        navigate(`/students/${id}`)
    }


    if(loading){
        return(
            <div className="container py-3">
                Loading...
            </div>
        );
    }
    if(error && !form){
        return(
            <div className="container py-3">
                <div className="alert alert-danger">
                    {error}
                </div>
            </div>
        );
    }

    return(
        <div className="container py-3">
            <div className="d-flex justify-content-between align-items-center">
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
            <hr/>            

            <StudentForm
                value={form}
                onChange={setForm}
                onSubmit={handleSubmit}
                onCancel={handleCancel}
                saving={saving}
                error={error}
                submitText="Update"
            />



        </div>
    );
    
}