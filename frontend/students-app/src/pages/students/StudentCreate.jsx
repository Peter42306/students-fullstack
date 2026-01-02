import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { createStudent, uploadEnclosures } from "../../api/studentsApi";
import StudentForm from "../../components/students/StudentForm";

const empty = {firstName:'', lastName:'', email:'', dateOfBirth:'', enrollmentDate:'', notes:'',};

export default function StudentCreate() {

    const navigate = useNavigate();    

    const [form, setForm] = useState(empty);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');    
    const [filesToUpload, setFilesToUpload] = useState([]);


    async function onSubmit(e) {
        e.preventDefault();

        setSaving(true);
        setError('');

        try {
            const created =  await createStudent(form);
            const studentId = created?.id;

            if(!studentId){
                throw new Error("Create succeeded, but server did not return id.");
            }

            if(filesToUpload.length > 0){
                await uploadEnclosures(studentId, filesToUpload);
            }


            navigate('/students');
        } catch (err) {
            setError(err?.message ?? 'Create failed');
        } finally {
            setSaving(false);
        }
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
                onSubmit={onSubmit}
                onCancel={() => navigate('/students')}
                saving={saving}
                error={error}
                submitText="Save"
            />

            <div>
                <hr/>
                <label>Enclosures (optional)</label>
                <input
                    type="file"
                    className="form-control"
                    multiple
                    onChange={(e) => setFilesToUpload(Array.from(e.target.files ?? []))}
                    disabled={saving}
                />
                {filesToUpload.length > 0 && (
                    <div>
                        Selected: {filesToUpload.length} files
                    </div>
                )}
            </div>

        </div>
    );
}