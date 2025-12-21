import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { deleteStudent, getStudents } from "../../api/studentsApi";
import StudentTable from "../../components/students/StudentTable";
import StudentsEmptyTable from "../../components/students/StudentsEmptyTable";
import StudentsError from "../../components/students/StudentsError";
import StudentsLoading from "../../components/students/StudentsLoading";
import StudentSearchBar from "../../components/students/StudentSearchBar";

export default function StudentsAll() {

    const [students, setStudents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [search, setSearch] = useState('');

    async function load() {
        setLoading(true);
        setError('');

        try {
            const data = await getStudents(search);
            setStudents(data);
        } catch (err) {
            setError(err?.message ?? 'Unknown error')
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        load();
    },[])
    

    async function onDelete(id) {
        if(!confirm('Delete student?')){
            return;
        }

        try {
            await deleteStudent(id)
            await load();
        } catch (err) {
            alert(err?.message ?? 'Delete failed');
        }
    }

    return(
        <div className="container py-3">

            <div className="d-flex justify-content-between align-content-center">
                <h1 className="mb-0">Students</h1>            
                <div className="text-end">
                <Link 
                    to="/students/create" 
                    className="btn btn-outline-primary btn-fixed"                    
                >
                    Add
                </Link>
            </div>

            </div>
            
            <hr/>           

            <div className="d-flex flex-column gap-2">
                <StudentSearchBar search={search} onSearchChange={setSearch} onSearch={load}/>
                {/* {loading && <StudentsLoading/>} */}
                {error && <StudentsError error={error}/>}
                {!loading && !error && students.length === 0 &&<StudentsEmptyTable/>}
                {!loading && !error && students.length > 0 &&<StudentTable students={students} onDelete={onDelete}/>}
            </div>

        </div>
    );
}