const API_BASE = '/api/students';

async function handleResponse(res) {
    if(!res.ok){
        let message = res.statusText;

        try {
            const data = await res.json();
            message = data.message ?? message;
        } catch {
            // if not JSON remained message = res.statusText
        }

        throw new Error(message);
    }

    if(res.status === 204){
        return;
    }

    return res.json();
}

// /api/Students
export async function getStudents(search) {
    const url = search 
        ? `${API_BASE}?search=${encodeURIComponent(search)}` 
        : API_BASE;
    const res = await fetch(url);
    return handleResponse(res);
}

// /api/Students
export async function createStudent(dto) {
    const res = await fetch(API_BASE,{
        method:'POST',
        headers:{'Content-Type': 'application/json'},
        body:JSON.stringify(dto),
    });
    return handleResponse(res);
}

// /api/Students/{id}
export async function deleteStudent(id) {
    const res = await fetch(`${API_BASE}/${id}`,{method: 'DELETE'});
    if(res.status === 204){
        return;
    }
    return handleResponse(res);
}

// /api/Students/{id}
export async function getStudent(id) {
    const res = await fetch(`${API_BASE}/${id}`);
    return handleResponse(res);
}

// /api/Students/{id}
export async function updateStudent(id, dto) {
  const res = await fetch(`${API_BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  return handleResponse(res);
}