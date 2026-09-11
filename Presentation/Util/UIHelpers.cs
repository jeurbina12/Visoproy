using System;
using System.Windows.Forms;
using System.Reflection;
using Presentation.Util;
using Common.Cache;

namespace Presentation.Util
{
    public static class UIHelpers
    {
        /// <summary>
        /// Comprueba permiso y muestra mensaje estandarizado
        /// </summary>
        public static bool HasPermission(bool permisoFlag)
        {
            if (!permisoFlag)
            {
                MessageBox.Show(ConfCache.MSG_NO_AUTORIZADO, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Intenta abrir el formulario. Si el owner tiene un Panel llamado panelChildForm lo abrirá dentro de él.
        /// Si no, abrirá el formulario normalmente (no modal) y suscribirá al método Logout del owner si existe.
        /// </summary>
        public static Form OpenModule(Form owner, Type formType, Func<bool> permiso = null, bool preferPanel = true)
        {
            try
            {
                if (permiso != null && !permiso()) return null;

                // Intentar localizar un panel llamado panelChildForm en el owner
                Panel targetPanel = null;
                var t = owner.GetType();
                // buscar campo
                var field = t.GetField("panelChildForm", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    targetPanel = field.GetValue(owner) as Panel;
                }
                else
                {
                    var prop = t.GetProperty("panelChildForm", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (prop != null)
                        targetPanel = prop.GetValue(owner) as Panel;
                }

                Form result = null;
                if (preferPanel && targetPanel != null)
                {
                    result = (Form)Fun.AbrirFormularioInPanel(formType, targetPanel);
                }
                else
                {
                    result = (Form)Fun.AbrirFormulario(formType, false);

                    if (result != null)
                    {
                        // Suscribir al Logout del owner si existe
                        var logoutMethod = t.GetMethod("Logout", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (logoutMethod != null)
                        {
                            try
                            {
                                FormClosedEventHandler handler = (FormClosedEventHandler)Delegate.CreateDelegate(typeof(FormClosedEventHandler), owner, logoutMethod);
                                result.FormClosed += handler;
                            }
                            catch
                            {
                                // ignore
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                try { System.IO.Directory.CreateDirectory(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")); System.IO.File.AppendAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "error.log"), DateTime.Now.ToString("s") + " - " + ex.ToString() + Environment.NewLine); } catch { }
                MessageBox.Show("Error al abrir módulo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
