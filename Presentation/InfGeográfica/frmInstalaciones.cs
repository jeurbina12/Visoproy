using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain;
using Common.Cache;
using Microsoft.VisualBasic;
using System.IO;
using System.Xml;
using Presentation.Util;
using System.Globalization;


namespace Presentation.InfGeográfica
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmInstalaciones : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        TreeviewModel treeviewModel = new TreeviewModel();        
        CadModel cadModel = new CadModel();
        
        System.Data.DataSet dataSetArbol;

        TreeNode nodeCortado = null;
        bool B_PEGAR = false;

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmInstalaciones()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();

            //CrearDataSet();
            treeView.AllowDrop = true;
            //
            // Se asigna el evento para detectar la seleccion del check del nodo
            //
            treeView.AfterCheck += treeView_AfterCheck;
            //treeView.AfterSelect += treeView_AfterSelect;            

            treeView.DragEnter += treeView_DragEnter;
            treeView.DragOver += treeView_DragOver;
            treeView.DragDrop += treeView_DragDrop;
            treeView.ItemDrag += treeView_ItemDrag;
            
            treeView.KeyUp += treeView_KeyUp;
            treeView.MouseClick += treeView_MouseClick;
            treeView.DoubleClick += treeView_DoubleClick;
            treeView.AfterSelect += treeView_AfterSelect;

            cadModel.CAPA_TEMP = "Marcar";
            
            //set the LinesColor proprerty
            //treeView.LineColor = System.Drawing.Color.Orange;
            //mnuOrden.PerformClick();
            //mnuAjustarV.PerformClick();
            //mnuVerFiltro.PerformClick();
            //rbn1x.PerformClick();
            //mnuActBlipmode.PerformClick();
        }

        private void FormInstalaciones_Load(object sender, EventArgs e)
        {
            ConfiguracionFormLectura();
            //this.MaximumSize = SystemInformation.PrimaryMonitorMaximizedWindowSize;
            btnCargar.PerformClick();
            //CrearNodosDelPadre(0, null);
            //LoadTreeView(treeView);                     

        }

        private void frmInstalaciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Fun.FormularioCerrar(typeof(frmSub));
            //Fun.FormularioCerrar(typeof(frmTransf));
            //Fun.FormularioCerrar(typeof(frmCto));

            ConfiguracionFormGuarda();
        }

        private void frmInstalaciones_Resize(object sender, EventArgs e)
        {
            if(WindowState.Equals(FormWindowState.Maximized))
                if (mnuOrden.Checked == true)
                     mnuOrden.PerformClick();          
        }

        private void CrearDataSet()
        {
            //var treeviewModel = new TreeviewModel();
            dataSetArbol = treeviewModel.ds_lista("esq_red.v_treeview2");
        }
        
        private void CrearDataSet_cto(int id_cto)
        {
            //var treeviewModel = new TreeviewModel();
            dataSetArbol = treeviewModel.ds_recursive_cto(id_cto);
        }

        private void CrearDataSet_id_roots(int id)
        {
            //var treeviewModel = new TreeviewModel();
            dataSetArbol = treeviewModel.ds_recursive_id_roots(id);
        }

        private void CrearNodosDelPadre(int indicePadre, TreeNode nodePadre)
        {
            // Crear un DataView con los Nodos que dependen del Nodo padre pasado como parámetro.
            DataView dataViewHijos = new DataView(dataSetArbol.Tables[0]);
            dataViewHijos.RowFilter = string.Format("{0} = {1}", dataSetArbol.Tables[0].Columns[2].ColumnName, indicePadre);

            // Agregar al TreeView los nodos Hijos que se han obtenido en el DataView.
            foreach ( DataRowView dataRowCurrent in dataViewHijos )
            {
                TreeNode nuevoNodo = new TreeNode();
                nuevoNodo.Name = dataRowCurrent["id"].ToString();
                nuevoNodo.Text = dataRowCurrent["nombre"].ToString().TrimEnd();
                nuevoNodo.ImageIndex = Int16.Parse(dataRowCurrent["id_icono"].ToString());
                nuevoNodo.SelectedImageIndex = Int16.Parse(dataRowCurrent["id_icono"].ToString());
                // si el parámetro nodoPadre es nulo es porque es la primera llamada, son los Nodos
                // del primer nivel que no dependen de otro nodo.
                if (nodePadre == null)
                {
                    treeView.Nodes.Add(nuevoNodo);
                }
                // se añade el nuevo nodo al nodo padre.
                else
                {
                    nodePadre.Nodes.Add(nuevoNodo);
                }

                // Llamada recurrente al mismo método para agregar los Hijos del Nodo recién agregado.

                CrearNodosDelPadre(Int32.Parse(dataRowCurrent["id"].ToString()), nuevoNodo);
            }
        }

        /// <summary>
        /// Check that all child nodes of the parent node are selected
        /// </summary>
        /// <param name="n">Currently selected node</param>
        /// <param name="check"> is selected</param>
        private void cycleChild(TreeNode n, bool check)
        {
            if (n.Nodes.Count != 0)
            {
                foreach (TreeNode child in n.Nodes)
                {
                    child.Checked = check;
                    if (child.Nodes.Count != 0)
                    {
                        cycleChild(child, check);
                    }
                }
            }
        }

        /// <summary>
        /// Traverse the parent node, if the child node is selected, all are selected, if the child node is not selected, the parent node is not selected.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="check"></param>
        private void cycleParent(TreeNode n, bool check)
        {
            if (n.Parent != null)
            {
                if (nextCheck(n))
                {
                    n.Parent.Checked = true;
                }
                else
                {
                    n.Parent.Checked = false;
                }
                cycleParent(n.Parent, check);
            }
        }

        /// <summary>
        /// Determine if the pass node is fully selected
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private bool nextCheck(TreeNode n)
        {
            foreach (TreeNode node in n.Parent.Nodes)
            {
                if (node.Checked == false)
                {
                    return false;
                }
            }
            return true;
        }
        

        private void mnuMostrarNodos_Click(object sender, EventArgs e)
        {
            try{
            TreeNode node = this.treeView.SelectedNode;
            node.ExpandAll();
            }
            catch
            {           
                return;
            }

        }

        private void mnuOcultarNodos_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView.SelectedNode;
            node.Collapse();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //treeView.AfterCheck -= treeView_AfterSelect;
            treeView.Nodes.Clear();
            //CrearDataSet();
            LoadTreeView(treeView);
            //treeView.AfterCheck += treeView_AfterSelect;
            this.Cursor = Cursors.Default;
           
        }

        private bool IfChecked(Int16 num)
        {
            switch (num)
            {
                case 1:
                    return chkCS1.Checked;
                case 2:
                    return chkCS2.Checked;
                case 3:
                    return chkCS3.Checked;
                case 4:
                    return chkCS4.Checked;
                case 5:
                    return chkCS5.Checked;
                case 6:
                    return chkCS6.Checked;
                case 7:
                    return chkCS7.Checked;
                default:
                    return chkCS8.Checked;
            }
        }

        private bool IfCheckedVn(Int16 num)
        {
            switch (num)
            {
                case 10:
                    return chkV1.Checked;
                case 11:
                    return chkV2.Checked;
                case 12:
                    return chkV3.Checked; 
                default:
                    return chkV4.Checked;
            }
        }

        private void LoadTreeView(TreeView tvw)
        {

            string sis = "", sub = "", tx = "", cto = "";

            TreeNode nodoSis = new TreeNode();
            TreeNode nodoSub = new TreeNode();
            TreeNode nodoTx = new TreeNode();
            TreeNode nodoCto = new TreeNode();
            Int16 id_cs = 0,id_kv=0;
            //CheckBox chkCS_n;
            DataTable dtt = treeviewModel.dt_lista("esq_red.v_treeview", 0, false);

            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                DataRow filaM = dtt.Rows[i];

                id_cs = Int16.Parse(filaM["root"].ToString());
                id_kv = Int16.Parse(filaM["id_icono_cto"].ToString());
                if (IfChecked(id_cs) && IfCheckedVn(id_kv))
                {

                    if (sis != filaM["id_sist"].ToString())
                    {
                        sis = filaM["id_sist"].ToString();
                        nodoSis = new TreeNode();
                        nodoSis.Name = sis;
                        nodoSis.Text = filaM["sist"].ToString().TrimEnd();
                        nodoSis.ImageIndex = Int16.Parse(filaM["id_icono_sist"].ToString());
                        nodoSis.SelectedImageIndex = Int16.Parse(filaM["id_icono_sist"].ToString());

                        tvw.Nodes.Add(nodoSis);
                    }
                    if (sub != filaM["id_sub"].ToString())
                    {
                        sub = filaM["id_sub"].ToString();
                        nodoSub = new TreeNode();
                        nodoSub.Name = sub;
                        nodoSub.Text = filaM["sub"].ToString().TrimEnd();
                        nodoSub.ImageIndex = Int16.Parse(filaM["id_icono_sub"].ToString());
                        nodoSub.SelectedImageIndex = Int16.Parse(filaM["id_icono_sub"].ToString());
                        tvw.Nodes[sis].Nodes.Add(nodoSub);
                    }
                    if (tx != filaM["id_tx"].ToString())
                    {

                        tx = filaM["id_tx"].ToString();
                        nodoTx = new TreeNode();
                        nodoTx.Name = tx;
                        nodoTx.Text = filaM["tx"].ToString().TrimEnd();
                        nodoTx.ImageIndex = Int16.Parse(filaM["id_icono_tx"].ToString());
                        nodoTx.SelectedImageIndex = Int16.Parse(filaM["id_icono_tx"].ToString());
                        tvw.Nodes[sis].Nodes[sub].Nodes.Add(nodoTx);
                    }
                    if (cto != filaM["id_cto"].ToString())
                    {
                        cto = filaM["id_cto"].ToString();
                        nodoCto = new TreeNode();
                        nodoCto.Name = cto;
                        nodoCto.Text = filaM["cto"].ToString().TrimEnd();
                        nodoCto.ImageIndex = Int16.Parse(filaM["id_icono_cto"].ToString());
                        nodoCto.SelectedImageIndex = Int16.Parse(filaM["id_icono_cto"].ToString());
                        tvw.Nodes[sis].Nodes[sub].Nodes[tx].Nodes.Add(nodoCto);
                    }
                }

            }
            dtt.Dispose();
            dtt = null;
        }

        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = this.treeView.SelectedNode;

            if (mnuCargarNodos.Checked && node.Nodes.Count.Equals(0) && node.Level.Equals(3))
            //if (node.Nodes.Count.Equals(0) && node.Level>3)
            {
                this.Cursor = Cursors.WaitCursor;
                
                CrearDataSet_cto(int.Parse(node.Name));
                CrearNodosDelPadre(int.Parse(node.Name), node);
                //treeView.Refresh();                
                //node.ExpandAll();
                node.Expand();
                this.Cursor = Cursors.Default;
                
            }
            else if (mnuCargarForm.Checked && node.Level > 0 && (node.Level < 4 || Int16.Parse(node.ImageIndex.ToString()) > 21))
            {
                node.Expand();
                AbrirPanel(false);
            }

            //else if (Int16.Parse(node.ImageIndex.ToString()) > 21)
            //{
            //    this.Cursor = Cursors.WaitCursor;
            // string nombre= treeviewModel.SelectNombre(int.Parse(node.Name), Int16.Parse(node.ImageIndex.ToString()));
            // if (!nombre.Equals(null))
            // {
                 
            //     frmClientes childForm = (frmClientes)Fun.AbrirFormulario(typeof(frmClientes), false);
            //     childForm.cbxFiltro.Text = "Poste";
            //     childForm.cbx_ltaVariable.Text = nombre;
            //     childForm.btnFiltrar.PerformClick();
            //     //this.WindowState = FormWindowState.Minimized;
            // }
             //this.Cursor = Cursors.Default;
            //}

        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //if (tsbCadVerCheck.Checked)
                if (this.tsbCadMostrarCheck.Tag.Equals(0))
                    mnuCadMostrar.PerformClick();
            
            }
                  catch
            {
                return;
            }
             //if (this.tsbCadVerCheck.Tag == null)
             //{
             //    this.tsbCadVerCheck.Tag = 0;
             //    this.tsbCadVerCheck.Image = global::Presentation.Properties.Resources.Checked;
             //}
             //else
             //{
             //    //msgb
             //    this.tsbCadVerCheck.Tag = null;
             //    this.tsbCadVerCheck.Image = global::Presentation.Properties.Resources.Unchecked;
             //}
                

            //    // Need to add e.action! = treeviewaction.unknown does not trigger the event when loading the form
            //if (e.Action != TreeViewAction.Unknown)
            //{
            //                     // If the corresponding parent node is selected, the corresponding child nodes are all selected
            //    if (e.Node.Checked == true)
            //    {            
            //        cycleChild(e.Node, true);

            //        if (e.Node.Parent != null)
            //        {
            //                                     // If the parent node of the corresponding parent node is not selected, the parent node is not selected, if the corresponding child node is selected, the parent node is selected.      
            //            if (nextCheck(e.Node))
            //            {
            //                cycleParent(e.Node, true);
            //            }
            //            else
            //            {
            //                cycleParent(e.Node, false);
            //            }
            //        }
            //    }
            //                     // If the corresponding parent node is not selected, the corresponding child node and parent node are not selected
            //    if (e.Node.Checked == false)
            //    {
            //            cycleParent(e.Node, false);
            //            cycleChild(e.Node, false); 
            //    }
            //}

        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Need to add e.action! = treeviewaction.unknown does not trigger the event when loading the form
            if (e.Action != TreeViewAction.Unknown)
            {
                // If the corresponding parent node is selected, the corresponding child nodes are all selected
                if (e.Node.Checked == true)
                {
                    cycleChild(e.Node, true);

                    //if (e.Node.Parent != null)
                    //{
                    //    // If the parent node of the corresponding parent node is not selected, the parent node is not selected, if the corresponding child node is selected, the parent node is selected.      
                    //    if (nextCheck(e.Node))
                    //    {
                    //        cycleParent(e.Node, true);
                    //    }
                    //    else
                    //    {
                    //        cycleParent(e.Node, false);
                    //    }
                    //}
                }
                // If the corresponding parent node is not selected, the corresponding child node and parent node are not selected
                if (e.Node.Checked == false)
                {
                    //cycleParent(e.Node, false);
                    cycleChild(e.Node, false);
                }
            }
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Mueve el nodo arrastrado cuando se usa el botón izquierdo del mouse.  
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }

            //// Copie el nodo arrastrado cuando se usa el botón derecho del mouse.  
            //else if (e.Button == MouseButtons.Right)
            //{
            //    DoDragDrop(e.Item, DragDropEffects.Copy);
            //}
        }

        private void treeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            // Recuperar las coordenadas del cliente de la posición del mouse.  
            Point targetPoint = treeView.PointToClient(new Point(e.X, e.Y));

            // Seleccione el nodo en la posición del mouse.  
            treeView.SelectedNode = treeView.GetNodeAt(targetPoint);
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            // Recuperar las coordenadas del cliente de la ubicación de colocación.  
            Point targetPoint = treeView.PointToClient(new Point(e.X, e.Y));

            // Recupere el nodo en la ubicación de colocación.  
            TreeNode targetNode = treeView.GetNodeAt(targetPoint);

            // Recuperar el nodo que fue arrastrado.  
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            //if (
            MoverNodos(targetNode, draggedNode);
            //// Confirme que el nodo en la ubicación de colocación no es   
            //// el nodo arrastrado o un descendiente del nodo arrastrado.  
            //if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode) && !draggedNode.Parent.Equals(targetNode))
            //{
            //    if (targetNode.ImageIndex > 9 && targetNode.ImageIndex < 22)
            //    {
                   

            //        //draggedNode.Parent.Text
            //        // Si se trata de una operación de movimiento, elimine el nodo de su actual   
            //        // ubicación y agréguelo al nodo en la ubicación de colocación.  
            //        if (e.Effect == DragDropEffects.Move)
            //        {
            //            draggedNode.Remove();
            //            targetNode.Nodes.Add(draggedNode);
            //            //int id = int.Parse(draggedNode.Name);
            //            //int root = int.Parse(targetNode.Name);
            //            updateRoot(int.Parse(draggedNode.Name), int.Parse(targetNode.Name), Int16.Parse(draggedNode.ImageIndex.ToString()));
            //        }

            //        //// Si se trata de una operación de copia, clone el nodo arrastrado   
            //        //// y agregarlo al nodo en la ubicación de colocación.  
            //        //else if (e.Effect == DragDropEffects.Copy)
            //        //{
            //        //    targetNode.Nodes.Add((TreeNode)draggedNode.Clone());
            //        //}

            //        // Expande el nodo en la ubicación   
            //        // para mostrar el nodo descartado.  
            //        targetNode.Expand();
            //    }
            //}

        }

        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            // Verifique el nodo padre del segundo nodo.  
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            // Si el nodo padre no es nulo o igual al primer nodo,   
            // llama al método ContainsNode de forma recursiva utilizando el padre de   
            // el segundo nodo.  
            return ContainsNode(node1, node2.Parent);
        }
        private void updateRoot(int id, int root, Int16 id_bloque)
        {
            // Actualizar tabla
            var treeviewModel = new TreeviewModel(id, root, id_bloque);
            var result = treeviewModel.UpdateRoot(id, root, id_bloque);
                  
            //this.Close();
           
        
        }

       

        private void mnuActListaBanco_Click(object sender, EventArgs e)
        {
            // Actualizar tabla tbl_tramos
            this.Cursor = Cursors.WaitCursor;
            //var treeviewModel = new TreeviewModel();
            var result = treeviewModel.Update_tbl_tramos_clientes();
            this.Cursor = Cursors.Default;
            MessageBox.Show(result, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuMostrarCad_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView.SelectedNode;

            if (node.Level<1)
            {
                System.Media.SystemSounds.Beep.Play();
                lblObs.Text = "Opción no opermitida";
                return;
            }

            double[] SelXY = new double[2];
            SelXY = treeviewModel.SelectXY(int.Parse(node.Name), Int16.Parse(node.ImageIndex.ToString()));

            if (SelXY[0].Equals(0))
            {
                System.Media.SystemSounds.Beep.Play();
                lblObs.Text =ConfCache.MSG_XY_NULL;
                return;
            }

             Int16 _marcar=0;
             if (mnuActMarcar.Checked)  
                _marcar = cadModel.MARCAR;     
             
            if (cadModel.CoordZoom(SelXY[0], SelXY[1], _marcar))
            {
                //node.BackColor = Color.Yellow;
                if (mnuOrden.Checked == false)
                    this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                lblObs.Text = this.cadModel.MENSAJE;
            }

            //lblXY.Text = string.Format("N({0}),{1},{2}", node.Name,SelXY[0], SelXY[1]);
           
        }

        private bool bool_pegar()
        {
            try{
            if (treeView.SelectedNode.Level > 3)
            {
                mnuCortar.Enabled = true;
                mnuPegar.Enabled = B_PEGAR;
                return true;
                 }
            else
            {
                
                mnuCortar.Enabled = false;
                mnuPegar.Enabled = false;
                return false;
            }

            }
            catch
            {
                mnuCortar.Enabled = false;
                mnuPegar.Enabled = false;
                return false;
            }

        }

        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                bool_pegar();
            }

            //TreeNode node = this.treeView.SelectedNode;
            //if (mnuVerArbol.Checked.Equals(false) && node.Level > 0 && node.Level < 4)
            //{
            //    AbrirPanel();
            //}
           
        }
        //private void treeView_AfterSelect(object sender, MouseEventArgs e)
        //{
            //if (tsbCadVerCheck.Checked)
            //    mnuCadMostrar.PerformClick();

        //}

        private void mnuCortar_Click(object sender, EventArgs e)
        {
            B_PEGAR = true;
            nodeCortado = treeView.SelectedNode;
        }

        private bool MoverNodos(TreeNode targetNode, TreeNode draggedNode)
        {
             try
            {

                // Confirme que el nodo en la ubicación de colocación no es   
                // el nodo arrastrado o un descendiente del nodo arrastrado.  
                if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode) && !draggedNode.Parent.Equals(targetNode))
                {
                    if (targetNode.ImageIndex > 9 && targetNode.ImageIndex < 22 && draggedNode.ImageIndex > 9)//13
                    {
                        //draggedNode.Parent.Text // Recuperar el nodo que fue arrastrado.  
                        // Si se trata de una operación de movimiento, elimine el nodo de su actual   
                        // ubicación y agréguelo al nodo en la ubicación de colocación.  
                        //if (e.Effect == DragDropEffects.Move)
                        //{
                        draggedNode.Remove();
                        targetNode.Nodes.Add(draggedNode);
                        updateRoot(int.Parse(draggedNode.Name), int.Parse(targetNode.Name), Int16.Parse(draggedNode.ImageIndex.ToString()));
                        //}

                        //// Si se trata de una operación de copia, clone el nodo arrastrado   
                        //// y agregarlo al nodo en la ubicación de colocación.  
                        //else if (e.Effect == DragDropEffects.Copy)
                        //{
                        //    targetNode.Nodes.Add((TreeNode)draggedNode.Clone());
                        //}

                        // Expande el nodo en la ubicación   
                        // para mostrar el nodo descartado.  

                        targetNode.Expand();
                    }
                    else
                    {
                        MessageBox.Show("Cambio no permitido", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                }

                return true;
               
             }
                  catch
            {
                return false;
            }
        }
        private void mnuPegar_Click(object sender, EventArgs e)
        {
            // nodo de colocación.  
            TreeNode NodePegar = treeView.SelectedNode;

            // nodo que fue cortado.  
            TreeNode NodeCortado = nodeCortado;

            B_PEGAR = !MoverNodos(NodePegar,NodeCortado);
        }

        private void treeView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                if (treeView.SelectedNode != null)
                {
                    e.Handled = true;
                    this.KeyPreview = true;

                    //copy node label to clipboard
                    Clipboard.SetText(treeView.SelectedNode.Text);
                }
            }
            else if (e.Control && e.KeyCode.ToString() == "X" && bool_pegar())
            {
                //e.Handled = true;
                //this.KeyPreview = true;
                 //mnuCortar.PerformClick();
                B_PEGAR = true;
                nodeCortado = treeView.SelectedNode;
            }
            else if (e.Control && e.KeyCode.ToString() == "V" && B_PEGAR)
            {
                //e.Handled = true;
                //this.KeyPreview = true;
                mnuPegar.PerformClick();
            }
        }

        private void mnuTotHijos_Click(object sender, EventArgs e)
        {
            // Get the count of the child tree nodes contained in the SelectedNode.
            int myNodeCount = treeView.SelectedNode.GetNodeCount(true);
            // Display the tree node path and the number of child nodes it and the tree view have.
            MessageBox.Show("El Nodo '" + treeView.SelectedNode.FullPath + "' tienen "
              + myNodeCount.ToString() + " hijos conectados.", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuTotTxFs_Click(object sender, EventArgs e)
        {
            int ncheck = 0;
            //Se Declara una colección de nodos apartir de tu Treeview
            //del que se va a recorrer            
                TreeNodeCollection nodes = treeView.Nodes;
                //Se recorren los nodos principales
                foreach (TreeNode n in nodes)
                {
                    //Se Declara un metodo para que recorra los hijos de los principales
                    //Y los hijos de los hijos....Recorrido Total en pocas palabras
                    //Para ello se envía el nodo actual para evaluar si tiene hijos
                   ncheck+= RecorrerNodosTx(n);
                }
                MessageBox.Show("Total Bancos Tx Afectados: " + ncheck, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int RecorrerNodosTx(TreeNode treeNode)
        {
            try
            {
                int ncheck = 0;
                //Si el nodo que recibimos tiene hijos se recorrerá
                //para luego verificar si esta o no checado
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    //Se Verifica si esta marcado...
                    if (tn.Checked == true && tn.ImageIndex>21)                  
                        ncheck += 1; 
                    //Ahora hago verificacion a los hijos del nodo actual
                    //Esta iteración no acabara hasta llegar al ultimo nodo principal
                    ncheck += RecorrerNodosTx(tn);
                }
                return ncheck;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }

        private double RecorrerNodoskVA(TreeNode treeNode)
        {
            try
            {
                double ncheck = 0;


                //Si el nodo que recibimos tiene hijos se recorrerá
                //para luego verificar si esta o no checado
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    //Se Verifica si esta marcado...
                    if (tn.Checked == true && tn.ImageIndex > 21 && tn.ImageIndex!=26)
                    {

                        ncheck += treeviewModel.kVAinst(tn.Text);
                        //Si esta marcado mostramos el texto del nodo
                        //MessageBox.Show(tb.Text);
                    }
                    //Ahora hago verificacion a los hijos del nodo actual
                    //Esta iteración no acabara hasta llegar al ultimo nodo principal
                    ncheck += RecorrerNodoskVA(tn);
                }
                return ncheck;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }

        private double RecorrerNodosTransf(TreeNode treeNode)
        {
            try
            {
                double ncheck = 0;


                //Si el nodo que recibimos tiene hijos se recorrerá
                //para luego verificar si esta o no checado
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    //Se Verifica si esta marcado...
                    if (tn.Checked == true && tn.ImageIndex > 21 && tn.ImageIndex!=26)                  
                        ncheck += treeviewModel.nTransf(tn.Text);

                    ncheck += RecorrerNodosTransf(tn);
                }
                return ncheck;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }

        private int nClientes(string texto)
        {
            char[] delimiterChars = { ' ' };//{ ' ', ',', '.', ':', '\t' };
            string[] elementos;
           
            elementos = texto.Split(delimiterChars);
            string mytext = elementos[2].Substring(1).TrimEnd(')');
            return int.Parse(mytext);
        }
        private int RecorrerNodosClientes(TreeNode treeNode)
        {
            try
            {
                int ncheck = 0;
                //Si el nodo que recibimos tiene hijos se recorrerá
                //para luego verificar si esta o no checado
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    //Se Verifica si esta marcado...
                    if (tn.Checked == true && tn.ImageIndex > 21 && tn.ImageIndex!=26)                   
                        ncheck += nClientes(tn.Text);                
                    ncheck += RecorrerNodosClientes(tn);
                }
                return ncheck;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }

        private void mnuTotClientesFs_Click(object sender, EventArgs e)
        {
            int ncheck = 0;
            //Se Declara una colección de nodos apartir de tu Treeview
            //del que se va a recorrer            
            TreeNodeCollection nodes = treeView.Nodes;
            //Se recorren los nodos principales
            foreach (TreeNode n in nodes)
            {
                //Se Declara un metodo para que recorra los hijos de los principales
                //Y los hijos de los hijos....Recorrido Total en pocas palabras
                //Para ello se envía el nodo actual para evaluar si tiene hijos
                ncheck += RecorrerNodosClientes(n);
            }
            MessageBox.Show("Total Clientes Afectados: " + ncheck, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

       

        

        private void mnuTotkVAFs_Click(object sender, EventArgs e)
        {
            double ncheck = 0;
            //Se Declara una colección de nodos apartir de tu Treeview
            //del que se va a recorrer            
            TreeNodeCollection nodes = treeView.Nodes;
            //Se recorren los nodos principales
            foreach (TreeNode n in nodes)
            {  
                ncheck += RecorrerNodoskVA(n);
            }
            MessageBox.Show("Total kVA Instalados Afectados: " + ncheck, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private TreeNode GetNodeByName(TreeNodeCollection nodes, string search_name)
        {
            TreeNode n_found_node = null;
            bool b_node_found = false;

            foreach (TreeNode node in nodes)
            {
                if (node.Name.Equals(search_name))
                {
                    b_node_found = true;
                    n_found_node = node;

                    return n_found_node;
                }

                if (!b_node_found)
                {
                    n_found_node = GetNodeByName(node.Nodes, search_name);

                    if (n_found_node != null)
                    {
                        //b_node_found = true;
                        //n_found_node = node;
                        return n_found_node;
                    }
                }
            }
            return null;
        }

        private void mnuInstAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                int id_padre = int.Parse(this.treeView.SelectedNode.Name);
                int id_icono_padre = this.treeView.SelectedNode.ImageIndex;
                
                //agregar solo en equipos Seccionadores y Cortacorrientes
                if (id_icono_padre > 9 && id_icono_padre < 22)//13
                {                   
                    if (mnuOrden.Checked == false)
                    this.WindowState = FormWindowState.Minimized;

                    if (cadModel.BloqueLeer())
                    {
                        //string s_tabla=treeviewModel.NombreTabla(cadModel.

                        if (cadModel.CAPA.Equals("TRANS") || cadModel.CAPA.Equals("RED"))
                        {
                            if (treeviewModel.AgregarInstalación(id_padre, cadModel.CODIGO, cadModel.ESC, cadModel.X, cadModel.Y, cadModel.GRADOS, cadModel.CAPACIDAD, cadModel.SIVPLUS, cadModel.BLOQUE, cadModel.CAPA))
                            {
                                //lblXY.Text = string.Format("{0},{1}", cadModel.X, cadModel.Y);
                                //lblXY.Text = string.Format("N({0}),{1},{2}", treeviewModel.Id, cadModel.X, cadModel.Y);
                            }
                            else
                            {
                                lblObs.Text = treeviewModel.MENSAJE;
                                MessageBox.Show(treeviewModel.MENSAJE, "Error de Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (mnuOrden.Checked == false)
                                {
                                    this.WindowState = FormWindowState.Normal;
                                    this.TopMost = true;
                                }
                                return;
                            }

                        }
                        else
                        {                            
                            lblObs.Text = "Capa de objeto no corresponde a la Red: " + cadModel.CAPA;
                            return;
                        }
                    }
                    else
                    {                       
                        lblObs.Text = cadModel.MENSAJE;
                        return;
                    }
             
                    lblObs.Text = cadModel.MENSAJE+", "+treeviewModel.MENSAJE;

                    if (treeviewModel.ID > 0)
                    {
                        // nodo de colocación.  
                        TreeNode NodePegar = treeView.SelectedNode;
                            nodeCortado = GetNodeByName(this.treeView.Nodes, treeviewModel.ID.ToString());

                            if (nodeCortado != null)
                            {
                                // nodo que fue cortado.  
                                TreeNode NodeCortado = nodeCortado;
                                B_PEGAR = !MoverNodos(NodePegar, NodeCortado);
                            }
                            else
                            {
                                TreeNode nuevoNodo = new TreeNode();

                                    nuevoNodo.Name = treeviewModel.ID.ToString();
                                    nuevoNodo.Text = treeviewModel.S_NOMBRE;
                                    nuevoNodo.ImageIndex = treeviewModel.ID_ICONO;
                                    nuevoNodo.SelectedImageIndex = treeviewModel.ID_ICONO;

                                    NodePegar.Nodes.Add(nuevoNodo);
                                    NodePegar.Expand();

                                    if (!treeviewModel.B_NUEVO)
                                    lblObs.Text += ", Instalación Movida de otro Circuito"; 
                            }

                        if (mnuActContinuo.Checked)
                            mnuInstAgregar.PerformClick();//proceso recursivo

                        if (mnuOrden.Checked == false)
                        this.WindowState = FormWindowState.Normal;
                    }
                }
                else
                {                    
                    lblObs.Text = cadModel.MENSAJE;
                    MessageBox.Show("No esta permitido agregar Instalación al objeto seleccionado", "Error de Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //if (mnuOrden.Checked == true)
                //{
                //    this.TopMost = true;

                //}
                //this.TopLevel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void mnuOrden_Click(object sender, EventArgs e)
        {
            if (mnuOrden.Checked == true)
            {
                mnuOrden.Checked = false;
                btnOrden.Checked = false;
                this.TopMost = false;
            }
            else
            {
                mnuOrden.Checked = true;
                btnOrden.Checked = true;
                this.TopMost = true;
            }
        }

        private void mnuAjustarV_Click(object sender, EventArgs e)
        {
            
            // StartPosition was set to FormStartPosition.Manual in the properties window.
            //Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            //Rectangle screen = Screen.FromControl(this).Bounds;
       //int w = Width >= screen.Width ? screen.Width : (screen.Width + Width*0) / 3;
       //int h = Height >= screen.Height ? screen.Height : (screen.Height + Height*0) / 1;
       //this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
       //this.Size = new Size(w, h);
            this.WindowState = FormWindowState.Normal; ;
            this.Location = new Point(0, 0);
       this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Size.Width/3, Screen.PrimaryScreen.WorkingArea.Size.Height*9/10);
       //this.splitContainer1.SplitterDistance = 250;
       if (!mnuVerArbol.Checked)
       {
           mnuVerArbol.PerformClick();
           //splitContainer1.Panel2Collapsed = true;
       }
        }

        private void mnuTot_nTrans_Fs_Click(object sender, EventArgs e)
        {
            double ncheck = 0;
            //Se Declara una colección de nodos apartir de tu Treeview
            //del que se va a recorrer            
            TreeNodeCollection nodes = treeView.Nodes;
            //Se recorren los nodos principales
            foreach (TreeNode n in nodes)                         
                ncheck += RecorrerNodosTransf(n);
           
            MessageBox.Show("Total # Transformadores Afectados: " + ncheck, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuParkVAFs_Click(object sender, EventArgs e)
        {           
          double  ParkVAFs = RecorrerNodoskVA(treeView.SelectedNode);

          MessageBox.Show("kVA Instalados Afectados en el nodo seleccionado: " + ParkVAFs, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        //private void mnuCadCamNomInst_Click(object sender, EventArgs e)
        //{
        //    if (mnuOrden.Checked == false)
        //        this.WindowState = FormWindowState.Minimized;

        //    TreeNode node = this.treeView.SelectedNode;
        //    treeviewModel.S_Nombre = node.Text;
        //    lblObs.Text = treeviewModel.CambiarNombre(int.Parse(node.Name), Int16.Parse(node.ImageIndex.ToString()));

        //    if (!treeviewModel.S_Nombre.Equals("-1"))
        //    {
        //        node.Text = treeviewModel.S_Nombre;
        //        node.ImageIndex = treeviewModel.Id_Icono;
        //        node.SelectedImageIndex = treeviewModel.Id_Icono;

        //    }
                       
            
        //        this.WindowState = FormWindowState.Normal;  
        //}
        private void UbicarNodo(string codigo, string bloque)
        {
            if (treeviewModel.UbicarInstalación(codigo,bloque))
            {               
                TreeNode TargetNode = GetNodeByName(this.treeView.Nodes, treeviewModel.ID.ToString());
                if (TargetNode != null)
                {

                    TargetNode.Expand();
                    treeView.SelectedNode = TargetNode;
                }
                else
                {
                    CrearDataSet_id_roots(treeviewModel.ID);
                   
                    string n_cto = System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 4]["id"]); ;
                    string msg = System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 1]["nombre"]).TrimEnd();
                    msg += "\\" + System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 2]["nombre"]).TrimEnd();
                    msg += "\\" + System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 3]["nombre"]).TrimEnd();
                    msg += "\\" + System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 4]["nombre"]).TrimEnd();
                    lblObs.Text = msg;
                    //if (mnuOrden.Checked == false)
                    //this.WindowState = FormWindowState.Normal;
                    //this.TopMost = true;
                    DialogResult result = MessageBox.Show(string.Format("Para ubicar el Nodo: {0}\nEs necesario cargar la Red del cto:\n{1}", treeviewModel.ID, msg), "Desea Cargar la Red en el árbol?", MessageBoxButtons.YesNo);


                    if (result == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        TreeNode TargetNode2 = GetNodeByName(this.treeView.Nodes, n_cto);
                        if (TargetNode2 != null)
                        {
                            CrearDataSet_cto(int.Parse(n_cto));
                            CrearNodosDelPadre(int.Parse(n_cto), TargetNode2);
                            TargetNode2.Expand();
                            TargetNode = GetNodeByName(this.treeView.Nodes, treeviewModel.ID.ToString());
                            TargetNode.Expand();
                            treeView.SelectedNode = TargetNode;
                            //treeView.Refresh();

                            //TargetNode2.ExpandAll();
                            //TargetNode2.Expand();
                            //treeView.SelectedNode = TargetNode;
                        }

                        this.Cursor = Cursors.Default;

                    }
                }

                //lblXY.Text = string.Format("{0},{1}", cadModel.X, cadModel.Y);
                //lblXY.Text = string.Format("N({0}),{1},{2}", treeviewModel.Id, cadModel.X, cadModel.Y);
            }
            else
            {
                lblObs.Text = treeviewModel.MENSAJE;
                MessageBox.Show("Instalación no registrada en la base de datos: " + cadModel.CODIGO, "Objecto no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;


            }
        }

        private void mnuCadUbicar_Click(object sender, EventArgs e)
        {
            try
            {
                if (mnuOrden.Checked == false)
                    this.WindowState = FormWindowState.Minimized;

                if (cadModel.BloqueLeer())
                {
                    if (cadModel.CAPA.Equals("TRANS") || cadModel.CAPA.Equals("RED"))
                    {
                         //treeviewModel.Id_Icono = cadModel.Id_Icono(cadModel.BLOQUE);
                         //treeviewModel.Id_bloque = cadModel.Id_Bloque(cadModel.BLOQUE);
                        UbicarNodo(cadModel.CODIGO, cadModel.BLOQUE);
                        //if (treeviewModel.UbicarInstalación(cadModel.CODIGO, cadModel.BLOQUE))
                        //{
                        //    //string TargetText = treeviewModel.Id.ToString();
                        //    //TreeNode TargetNode = treeView.Nodes.Cast<TreeNode>().ToList().Find(n => n.Name.Equals(TargetText));
                        //    //TreeNode TargetNode = GetNodeByName(this.treeView.Nodes, treeviewModel.Id.ToString());
                        //    TreeNode TargetNode = GetNodeByName(this.treeView.Nodes, treeviewModel.ID.ToString());
                        //    if (TargetNode != null)
                        //    {

                        //        TargetNode.Expand();
                        //        treeView.SelectedNode = TargetNode;
                        //    }
                        //    else
                        //    {
                        //        CrearDataSet_id_roots(treeviewModel.ID);
                        //        // Crear un DataView con los Nodos que dependen del Nodo padre pasado como parámetro.
                        //        //DataView dataViewHijos = new DataView(dataSetArbol.Tables[0]);

                        //        //for (int i = this.dataSetArbol.Tables[0].Rows.Count; i < this.dataSetArbol.Tables[0].Rows.Count - 3; i--)
                        //        //{
                        //        //    variable = System.Convert.ToString(this.ds.Tables[0].Rows[i]["nombre"]).TrimEnd();
                        //        //    cbx_ltaVariable.Items.Add(variable);
                        //        //}
                        //        //this.dataSetArbol.Tables[0].Rows.Count-1
                        //        string n_cto = System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 4]["id"]); ;
                        //        string msg = System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 1]["nombre"]).TrimEnd();
                        //        msg += "\\" + System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 2]["nombre"]).TrimEnd();
                        //        msg += "\\" + System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 3]["nombre"]).TrimEnd();
                        //        msg += "\\" + System.Convert.ToString(this.dataSetArbol.Tables[0].Rows[this.dataSetArbol.Tables[0].Rows.Count - 4]["nombre"]).TrimEnd();
                        //        lblObs.Text = msg;
                        //        //if (mnuOrden.Checked == false)
                        //        this.WindowState = FormWindowState.Normal;
                        //        this.TopMost = true;
                        //         DialogResult result = MessageBox.Show(string.Format("Para ubicar el Nodo: {0}\nEs necesario cargar la Red del cto:\n{1}", treeviewModel.ID, msg), "Desea Cargar la Red en el árbol?", MessageBoxButtons.YesNo);

                                
                        //        if (result == DialogResult.Yes)
                        //        {
                        //            this.Cursor = Cursors.WaitCursor;
                        //            TreeNode TargetNode2 = GetNodeByName(this.treeView.Nodes, n_cto);
                        //            if (TargetNode2 != null)
                        //            {                                      
                        //                CrearDataSet_cto(int.Parse(n_cto));
                        //                CrearNodosDelPadre(int.Parse(n_cto), TargetNode2);
                        //                TargetNode2.Expand();
                        //                TargetNode = GetNodeByName(this.treeView.Nodes, treeviewModel.ID.ToString());
                        //                TargetNode.Expand();
                        //                treeView.SelectedNode = TargetNode;
                        //                //treeView.Refresh();
                                       
                        //                //TargetNode2.ExpandAll();
                        //                //TargetNode2.Expand();
                        //                //treeView.SelectedNode = TargetNode;
                        //            }

                        //            this.Cursor = Cursors.Default;
                                    
                        //        }
                        //    }

                        //    //lblXY.Text = string.Format("{0},{1}", cadModel.X, cadModel.Y);
                        //    //lblXY.Text = string.Format("N({0}),{1},{2}", treeviewModel.Id, cadModel.X, cadModel.Y);
                        //}
                        //else
                        //{
                        //    lblObs.Text = treeviewModel.MENSAJE;
                        //    MessageBox.Show("Instalación no registrada en la base de datos: " + cadModel.CODIGO, "Objecto no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //    return;


                        //}
                        if (mnuOrden.Checked == false)
                        this.WindowState = FormWindowState.Normal;
                        //this.TopMost = true;
                    }
                    else
                    {
                        lblObs.Text = cadModel.MENSAJE;
                        MessageBox.Show("Capa de objeto no corresponde a la Red: " + cadModel.CAPA, "Error de Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return;

                    }
                }else
                    lblObs.Text = cadModel.MENSAJE;
                //int id_padre = int.Parse(this.treeView.SelectedNode.Name);
                //int id_image_padre = this.treeView.SelectedNode.ImageIndex;
                //agregar solo en equipos Seccionadores y Cortacorrientes
                //if (id_image_padre > 13 && id_image_padre < 22)
                //{
                    //if (mnuOrden.Checked == false)
                    //    this.WindowState = FormWindowState.Minimized;

                    //var treeviewModel = new TreeviewModel();
                    //lblObs.Text = treeviewModel.UbicarInstalación(id_padre) + ", ID: " + treeviewModel.Id;
                    //if (treeviewModel.Id > 0)
                    //{
                       
                      
                        //// nodo de colocación.  
                        //TreeNode NodePegar = treeView.SelectedNode;

                        //// nodo que fue cortado.  
                        //TreeNode NodeCortado = nodeCortado;

                        //B_PEGAR = !MoverNodos(NodePegar, NodeCortado);


                        //nodeCortado = treeView.SelectedNode;
                        
                        //treeView.Nodes.Remove(TargetNode);

                        //TreeNode tn = treeView.Nodes.OfType<TreeNode>().Where(x => x.Name.Equals(treeviewModel.Id.ToString())).FirstOrDefault();
                        //TreeNode tn = new TreeNode();
                        //tn = treeView.Nodes.IndexOfKey(treeviewModel.Id.ToString());
                        //tn = treeView.Nodes.Find(treeviewModel.Id, true);

                        //TreeNode nuevoNodo = new TreeNode();
                        //nuevoNodo.Name = treeviewModel.Id.ToString();
                        //nuevoNodo.Text = treeviewModel.S_Nombre;
                        //nuevoNodo.ImageIndex = treeviewModel.id_Icono;
                        //nuevoNodo.SelectedImageIndex = treeviewModel.id_Icono;
                        //this.treeView.SelectedNode.Nodes.Add(nuevoNodo);
                        //this.treeView.SelectedNode.Expand();

                        //if (nodeCortado != null)
                        //{
                        //    treeView.Nodes.Remove(nodeCortado);
                        //}



                    if (mnuOrden.Checked == false)
                        this.WindowState = FormWindowState.Normal;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

       

      

        private void tsbCadMostrarCheck_Click(object sender, EventArgs e)
        {
            //if (tsbCadVerCheck.Checked)
            //{
            //System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            //string resourceName = asm.GetName().Name + ".Resources.Checked.bmp";
                //tsbCadVerCheck.Checked = !tsbCadVerCheck.Checked;
                //string dir = Path.GetDirectoryName(Application.ExecutablePath);
                //string filename = Path.Combine(dir, @"Checked.bmp");

                //tsbCadVerCheck.Image = Image.FromFile(filename);
            //tsbCadVerCheck.Image = Image.FromFile(@"C:\VSG\iconos\Checked.bmp");global::Presentation.Properties.Resources.
                  

            mnuActZoom.PerformClick();
                
                //tsbCadMarcar.Image = GetImageByName("Checked.bmp");
            //}
        }
                     

       

        private void tsbCadAgreInst_Click(object sender, EventArgs e)
        {
            mnuInstAgregar.PerformClick();
        }
        
       

        private void mnuCadActInst_Click(object sender, EventArgs e)
        {
            if (mnuOrden.Checked == false)
                this.WindowState = FormWindowState.Minimized;
            //CARGAR DATOS DEL NODO
            TreeNode node = this.treeView.SelectedNode;
            treeviewModel.ID = int.Parse(node.Name);
           treeviewModel.ID_ICONO =Convert.ToInt16(node.ImageIndex);
            //treeviewModel.S_Tabla=treeviewModel.NombreTabla(treeviewModel.Id_Icono);
            treeviewModel.S_NOMBRE = node.Text;

             //solo en tramos y cargas
            if (treeviewModel.ID_ICONO > 13)//13
            {
                if (mnuOrden.Checked == false)
                    this.WindowState = FormWindowState.Minimized;

                if (cadModel.BloqueLeer())
                {
                    if (cadModel.CAPA.Equals("TRANS") || cadModel.CAPA.Equals("RED"))
                    {

                        if (treeviewModel.ActualizarInst(cadModel.CODIGO, cadModel.ESC, cadModel.X, cadModel.Y, cadModel.GRADOS, cadModel.CAPACIDAD, cadModel.SIVPLUS, cadModel.BLOQUE))
                        {                            
                            //lblXY.Text = string.Format("N({0}),{1},{2}", treeviewModel.Id, cadModel.X, cadModel.Y);
                        }

                    }
                    else
                    {
                        lblObs.Text = "No corresponde a la Capa : RED,TRANS <>" + cadModel.CAPA;
                        MessageBox.Show(lblObs.Text, "Error de Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    if (mnuOrden.Checked == false)
                    {
                        this.WindowState = FormWindowState.Normal;
                        this.TopMost = true;
                    }                   
                    lblObs.Text = cadModel.MENSAJE;
                    MessageBox.Show(lblObs.Text, "Error de Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                if (mnuOrden.Checked == false)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.TopMost = true;
                    //this.TopLevel = true;
                }
                lblObs.Text = "Solo se permiten actualizar Tramos y Cargas";
                MessageBox.Show(lblObs.Text, "Cambio no permitido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;            
            }
            lblObs.Text = treeviewModel.MENSAJE;

                node.Text = treeviewModel.S_NOMBRE;
                node.ImageIndex = treeviewModel.ID_ICONO;
                node.SelectedImageIndex = treeviewModel.ID_ICONO;
                      
                if (mnuOrden.Checked == false)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.TopMost = true;
                }
        }

       

        private void btnZoom_Click(object sender, EventArgs e)
        {
            Int16 _marcar = 0;
            if (mnuActMarcar.Checked)
                _marcar = cadModel.MARCAR;

            if (cadModel.ZoomGeo(txtCodigo.Text, _marcar))
            {
                if (mnuOrden.Checked == false)
                    this.WindowState = FormWindowState.Minimized;   
            }
            lblObs.Text = cadModel.MENSAJE;           
        }

        private void btnCadUbicar_Click(object sender, EventArgs e)
        {
            mnuCadUbicar.PerformClick();
        }
             

        private void mnuVerArbol_Click(object sender, EventArgs e)
        {
            mnuVerArbol.Checked = !mnuVerArbol.Checked;
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }

        private void chkCS1_Click(object sender, EventArgs e)
        {
            chkCS1.Checked = !chkCS1.Checked;
        }

        private void chkCS2_Click(object sender, EventArgs e)
        {
            chkCS2.Checked = !chkCS2.Checked;
        }

        private void chkCS3_Click(object sender, EventArgs e)
        {
            chkCS3.Checked = !chkCS3.Checked;
        }

        private void chkCS4_Click(object sender, EventArgs e)
        {
            chkCS4.Checked = !chkCS4.Checked;
        }

        private void chkCS5_Click(object sender, EventArgs e)
        {
            chkCS5.Checked = !chkCS5.Checked;
        }

        private void chkCS6_Click(object sender, EventArgs e)
        {
            chkCS6.Checked = !chkCS6.Checked;
        }

        private void chkCS7_Click(object sender, EventArgs e)
        {
            chkCS7.Checked = !chkCS7.Checked;
        }

        private void chkCS8_Click(object sender, EventArgs e)
        {
            chkCS8.Checked = !chkCS8.Checked;
        }

        private void chkV1_Click(object sender, EventArgs e)
        {
            chkV1.Checked = !chkV1.Checked;
        }

        private void chkV2_Click(object sender, EventArgs e)
        {
            chkV2.Checked = !chkV2.Checked;
        }

        private void chkV3_Click(object sender, EventArgs e)
        {
            chkV3.Checked = !chkV3.Checked;
        }

        private void chkV4_Click(object sender, EventArgs e)
        {
            chkV4.Checked = !chkV4.Checked;
        }
        //private void MarcarZoom(RadioButton control)
        //{
       


        //}

        private void rbn1x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn1x.Text;
            cadModel.ZOOM = 1;
        }

        private void rbn2x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn2x.Text;
            cadModel.ZOOM = 2;
        }

        private void rbn3x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn3x.Text;
            cadModel.ZOOM = 3;
        }

        private void rbn4x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn4x.Text;
            cadModel.ZOOM = 4;
        }

        private void tsbCadMostrar_ButtonClick(object sender, EventArgs e)
        {
            mnuCadMostrar.PerformClick();
        }

        private void mnuActBlipmode_Click(object sender, EventArgs e)
        {            
            if (mnuActBlipmode.Checked == true)
            {
                mnuActBlipmode.Checked = false;
                if (!cadModel.Activar_BLIPMODE("OFF"))
                {
                    lblObs.Text = "No se logró desactivar el comando BLIPMODE";
                }
            }
            else
            {
                mnuActBlipmode.Checked = true;
                if (!cadModel.Activar_BLIPMODE("ON"))
                {
                    lblObs.Text = "No se logró activar el comando BLIPMODE";
                }
            }
        }

       
        private void rbn5m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn5m.Text;
            cadModel.MARCAR = 5;
        }
        private void rbn10m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn10m.Text;
            cadModel.MARCAR = 10;
        }
        private void rbn15m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn15m.Text;
            cadModel.MARCAR = 15;
        }
        private void rbn20m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn0m.Text;
            cadModel.MARCAR = 20;
        }


        private void tsbCadLayerBorrar_Click(object sender, EventArgs e)
        {
            mnuCadLayerErase.PerformClick();        
              
        }

        private void mnuCadMarcarAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //if (tsbCadMarcar.Text.Equals("0 m"))
            //    cadModel.MARCAR=5;

            int ncheck = 0;
            //Se Declara una colección de nodos apartir de tu Treeview
            //del que se va a recorrer            
            TreeNodeCollection nodes = treeView.Nodes;
            //Se recorren los nodos principales
            foreach (TreeNode n in nodes)
           
                ncheck += RecorrerNodosMarcar(n);          


            lblObs.Text = "Total Puntos Marcados: " + ncheck;
            this.Cursor = Cursors.Default;
           
        }

        private int RecorrerNodosMarcar(TreeNode treeNode)
        {
            try
            {
                int ncheck = 0;
                //Si el nodo que recibimos tiene hijos se recorrerá
                //para luego verificar si esta o no checado
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    //Se Verifica si esta marcado...
                    if (tn.Checked == true && tn.ImageIndex > 21 && tn.ImageIndex!=26)
                    {
                        ncheck += 1;

                        double[] SelXY = new double[2];
                        SelXY = treeviewModel.SelectXY(int.Parse(tn.Name), Int16.Parse(tn.ImageIndex.ToString()));
                        cadModel.CoordMarcar(SelXY[0], SelXY[1]);                       
                    }
                    //Ahora hago verificacion a los hijos del nodo actual
                    //Esta iteración no acabara hasta llegar al ultimo nodo principal
                    ncheck += RecorrerNodosMarcar(tn);
                }
                return ncheck;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }

        private void tsbCadMarcar_ButtonClick(object sender, EventArgs e)
        {
            mnuCadMarcar.PerformClick();           
        }

        private void tsbCadMarcarAll_Click(object sender, EventArgs e)
        {
            mnuCadMarcarAll.PerformClick();
        }

        private void mnuCadLayerBorrar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!cadModel.LayerBorrar())
                lblObs.Text = cadModel.MENSAJE;

            this.Cursor = Cursors.Default;
        }

        private void mnuCadBloqDib_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView.SelectedNode;

            if (node.Level < 3)
            {
                System.Media.SystemSounds.Beep.Play();
                lblObs.Text = "Opción no opermitida";
                return;
            }

            string ntabla = treeviewModel.NombreTabla(Int16.Parse(node.ImageIndex.ToString()));
            DataSet ds_tbl;
            //string codigo,sivplus,capacidad,bloque;
            //double x, y,  escala;
            //Int16 grados;
            //cadModel.CAPA_TEMP="Marcar";

            if (ntabla.Equals("esq_red.tbl_cargas")){

                    ds_tbl=treeviewModel.ds_tbl_cargas(int.Parse(node.Name));
                   
                   cadModel.X = System.Convert.ToDouble(ds_tbl.Tables[0].Rows[0]["x"]);
                   cadModel.Y = System.Convert.ToDouble(ds_tbl.Tables[0].Rows[0]["y"]);
                   cadModel.CODIGO = System.Convert.ToString(ds_tbl.Tables[0].Rows[0]["nombre"]).TrimEnd();
                   cadModel.SIVPLUS = System.Convert.ToString(ds_tbl.Tables[0].Rows[0]["descripcion"]).TrimEnd();
                   cadModel.CAPACIDAD = System.Convert.ToString(ds_tbl.Tables[0].Rows[0]["kva_text"]).TrimEnd();
                   cadModel.GRADOS = System.Convert.ToInt16(ds_tbl.Tables[0].Rows[0]["grados"]);
                   cadModel.ESC = System.Convert.ToDouble(ds_tbl.Tables[0].Rows[0]["escala"]);
                   cadModel.BLOQUE = System.Convert.ToString(ds_tbl.Tables[0].Rows[0]["bloque"]).TrimEnd();
                   if (!cadModel.BloqueEscribir())
                       lblObs.Text = "No se logró Dibujar el Bloque";
            }
            else if (ntabla.Equals("esq_red.tbl_tramos"))
            {
                ds_tbl = treeviewModel.ds_tbl_tramos(int.Parse(node.Name));

                cadModel.X = Convert.ToDouble(ds_tbl.Tables[0].Rows[0]["x"]);
                cadModel.Y = Convert.ToDouble(ds_tbl.Tables[0].Rows[0]["y"]);
                cadModel.CODIGO = Convert.ToString(ds_tbl.Tables[0].Rows[0]["nombre"]).TrimEnd();
                cadModel.GRADOS = Convert.ToInt16(ds_tbl.Tables[0].Rows[0]["grados"]);
                cadModel.ESC = Convert.ToDouble(ds_tbl.Tables[0].Rows[0]["escala"]);
                cadModel.BLOQUE = Convert.ToString(ds_tbl.Tables[0].Rows[0]["bloque"]).TrimEnd();
                if (!cadModel.BloqueEscribir())
                    lblObs.Text = "No se pudo ingresar el Bloque";
            }
          
        }

        private void mnuEsc1_2_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1_2.Text;
            //if (!cadModel.bloq_esc(1.2))           
            //    lblObs.Text = "Función cancelada";            
            //else           
            //    if (mnuActContinuo.Checked)
            //        mnuEsc1_2.PerformClick();            
        }

        private void mnuActContinuo_Click(object sender, EventArgs e)
        {
            mnuActContinuo.Checked = !mnuActContinuo.Checked;

            if (mnuActContinuo.Checked)
            {
                cmdContinuo.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
                this.lblObs.Text = "Utilizar ESC para cancelar en AutoCad";
            }
            else
                cmdContinuo.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
        }

        private void mnuEsc1_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1.Text;

            //if (!cadModel.bloq_esc(1.2))
            //{
            //    lblObs.Text = "Función cancelada";
            //}
            //else
            //{

            //    if (mnuActContinuo.Checked)
            //        mnuEsc1.PerformClick();
            //}
            
        }

        private void ConfiguracionFormGuarda()
        {
            string nombreForm = this.Name;
            string ficheroXML = Path.Combine(ConfCache.RUTA_XML, nombreForm+".xml");

            XmlNode nodoPrincipal;


             try
            {
                // Intancia del documento XML que permitirá guardar la configuración
                XmlDocument doc = new XmlDocument();
                // Se comprueba que exista el fichero
                if (!File.Exists(ficheroXML))
                {
                    // No existe el fichero. Se añaden los nodos iniciales
                    doc.AppendChild(doc.CreateComment(ConfCache.MSG_CONF_FRM));

                    // Creación del nodo principal
                    nodoPrincipal = doc.CreateNode(XmlNodeType.Element, "Configuracion", null);

                    // Se añade el nodo principal al documento
                    doc.AppendChild(nodoPrincipal);
                }
                else
                {
                    // El fichero existe previamente. Se recupera la información
                    doc.Load(ficheroXML);

                    // Lectura del nodo principal
                    nodoPrincipal = doc.SelectSingleNode("Configuracion");
                }

                // Se comprueba si existe un nodo con el nombre del form actual
                XmlNode nodoFormActual = nodoPrincipal.SelectSingleNode(nombreForm);
                if (nodoFormActual == null)
                    nodoFormActual =
                          nodoPrincipal.AppendChild(
                            doc.CreateNode(XmlNodeType.Element, nombreForm, null));

                // Almacenamiento de los valores
             Fun.Instance.NodoValor(doc, nodoFormActual, "WindowState", this.WindowState);
             Fun.Instance.NodoValor(doc, nodoFormActual, "Top", this.Top);
             Fun.Instance.NodoValor(doc, nodoFormActual, "Left", this.Left);
             Fun.Instance.NodoValor(doc, nodoFormActual, "Width", this.Width);
             Fun.Instance.NodoValor(doc, nodoFormActual, "Height", this.Height);
             Fun.Instance.NodoValor(doc, nodoFormActual, "Orden", this.mnuOrden.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "VerArbol", this.mnuVerArbol.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerBarFiltro", this.mnuVerBarFiltro.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerBarCad", this.mnuVerBarCad.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "mnuCargarNodos", this.mnuCargarNodos.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "mnuCargarForm", this.mnuCargarForm.Checked); 
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkCS1", this.chkCS1.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkCS2", this.chkCS2.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkCS3", this.chkCS3.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkCS4", this.chkCS4.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkCS5", this.chkCS5.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkCS6", this.chkCS6.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkCS7", this.chkCS7.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkCS8", this.chkCS8.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkV1", this.chkV1.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkV2", this.chkV2.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkV3", this.chkV3.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "chkV4", this.chkV4.Checked);
             Fun.Instance.NodoValor(doc, nodoFormActual, "Zoom", cadModel.ZOOM);
             Fun.Instance.NodoValor(doc, nodoFormActual, "Marcar", cadModel.MARCAR);
             Fun.Instance.NodoValor(doc, nodoFormActual, "Codigo", txtCodigo.Text);
            
             
           
                // Se guarda el fichero de configuración en disco
                XmlTextWriter tw = new XmlTextWriter(ficheroXML, Encoding.UTF8);
                tw.Indentation = 4;
                tw.IndentChar = " "[0];
                tw.Formatting = Formatting.Indented;

                doc.Save(tw);
                doc = null;

                tw.Flush();
                tw.Close();

            }
             catch (Exception f)
             {

                 MessageBox.Show("Error: " + f.Message);

             }


        }

        /// <summary>
        /// Realiza una lectura del fichero de configuración y aplica los cambios al form
        /// </summary>
        private void ConfiguracionFormLectura()
        {
            string nombreForm = this.Name;
            string ficheroXML = Path.Combine(ConfCache.RUTA_XML, nombreForm + ".xml");

           
            XmlNode nodoPrincipal;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ficheroXML);

                // Lectura del nodo principal
                nodoPrincipal = doc.SelectSingleNode("Configuracion");

                // Lectura del nodo del formulario actual
                XmlNode nodoActual = nodoPrincipal.SelectSingleNode(nombreForm);

                // Lectura del modo de presentación de la ventana
                this.WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState),Fun.Instance.GetValor(nodoActual, "WindowState").ToString(), false);

                if (this.WindowState == FormWindowState.Normal)
                {
                    this.Top = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Top"));
                    this.Left = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Left"));
                    this.Width = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Width"));
                    this.Height = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Height"));

                    if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "Orden").ToString()))
                        this.mnuOrden.PerformClick();               
                   
                    //}
                    //else
                    //{
                    //    tsbOrden.Checked = false;
                }
                //this.mnuVerArbol.Checked = Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "VerArbol").ToString());
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuCargarNodos").ToString()))
                    mnuCargarNodos.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuCargarForm").ToString()))
                    mnuCargarForm.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "VerArbol").ToString()))
                    mnuVerArbol.PerformClick();

                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerBarFiltro").ToString()))
                    mnuVerBarFiltro.PerformClick();
                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerBarCad").ToString()))
                    mnuVerBarCad.PerformClick();   

                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkCS1").ToString()))
                    chkCS1.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkCS2").ToString()))
                    chkCS2.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkCS3").ToString()))
                    chkCS3.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkCS4").ToString()))
                    chkCS4.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkCS5").ToString()))
                    chkCS5.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkCS6").ToString()))
                    chkCS6.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkCS7").ToString()))
                    chkCS7.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkCS8").ToString()))
                    chkCS8.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkV1").ToString()))
                    chkV1.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkV2").ToString()))
                    chkV2.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkV3").ToString()))
                    chkV3.PerformClick();
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "chkV4").ToString()))
                    chkV4.PerformClick();
               
                cadModel.ZOOM = Convert.ToInt16(Fun.Instance.GetValor(nodoActual, "Zoom"));
                tsbCadMostrar.Text = cadModel.ZOOM + "x";
                
                cadModel.MARCAR = Convert.ToInt16(Fun.Instance.GetValor(nodoActual, "Marcar"));
                tsbCadMarcar.Text = cadModel.MARCAR+" m";


                txtCodigo.Text =Convert.ToString(Fun.Instance.GetValor(nodoActual, "Codigo"));
               
            }
            //catch (Exception f)
            catch 
            {
                XmlDocument doc = new XmlDocument();
                //XmlNode nodoPrincipal;
                // No existe el fichero. Se añaden los nodos iniciales
                doc.AppendChild(doc.CreateComment(ConfCache.MSG_CONF_FRM));

                // Creación del nodo principal
                nodoPrincipal = doc.CreateNode(XmlNodeType.Element, "Configuracion", null);

                // Se añade el nodo principal al documento
                doc.AppendChild(nodoPrincipal);

                cadModel.ZOOM = 1;
                cadModel.MARCAR = 5;
                tsbCadMarcar.Text = cadModel.MARCAR + " m";
            }
        }

        

        private void mnuVerBarFiltro_Click(object sender, EventArgs e)
        {
            mnuVerBarFiltro.Checked = !mnuVerBarFiltro.Checked;
            tspFiltros.Visible = mnuVerBarFiltro.Checked;
        }

        private void mnuVerBarCad_Click(object sender, EventArgs e)
        {
            mnuVerBarCad.Checked = !mnuVerBarCad.Checked;
            tspCad.Visible = mnuVerBarCad.Checked;
        }

        private void mnuCadMarcar_Click(object sender, EventArgs e)
        {
            TreeNode tn = this.treeView.SelectedNode;

            if (tn.Level < 3)
            {
                System.Media.SystemSounds.Beep.Play();
                //System.Media.SystemSounds.Asterisk.Play();
                //System.Media.SystemSounds.Exclamation.Play();
                //System.Media.SystemSounds.Question.Play();
                //System.Media.SystemSounds.Hand.Play();
                lblObs.Text = "Opción no opermitida";
                return;
            }

            double[] SelXY = new double[2];
            SelXY = treeviewModel.SelectXY(int.Parse(tn.Name), Int16.Parse(tn.ImageIndex.ToString()));
            if (!cadModel.CoordMarcar(SelXY[0], SelXY[1]))
                lblObs.Text = "Objeto no marcado: " + tn.Text;
        }

        private void mnuActZoom_Click(object sender, EventArgs e)
        {
            mnuActZoom.Checked = !mnuActZoom.Checked;

            if (this.tsbCadMostrarCheck.Tag == null)
            {
                this.tsbCadMostrarCheck.Tag = 0;
                this.tsbCadMostrarCheck.Image = global::Presentation.Properties.Resources.Checked;
            }
            else
            {
                //msgb
                this.tsbCadMostrarCheck.Tag = null;
                this.tsbCadMostrarCheck.Image = global::Presentation.Properties.Resources.Unchecked;
            }
        }

        private void mnuActMarcar_Click(object sender, EventArgs e)
        {
            mnuActMarcar.Checked = !mnuActMarcar.Checked;

            if (this.tsbCadMarcarCheck.Tag == null)
            {
                this.tsbCadMarcarCheck.Tag = 0;
                this.tsbCadMarcarCheck.Image = global::Presentation.Properties.Resources.Checked;
            }
            else
            {
                //msgb
                this.tsbCadMarcarCheck.Tag = null;
                this.tsbCadMarcarCheck.Image = global::Presentation.Properties.Resources.Unchecked;
            }
        }

        private void mnuInstQuitar_Click(object sender, EventArgs e)
        {
             try
            {
                 TreeNode targetNode =this.treeView.SelectedNode;

                 int id_padre = int.Parse(targetNode.Name);
                 string nombre = targetNode.Text;
                 Int16 id_icono_padre = Int16.Parse(targetNode.ImageIndex.ToString());

                //Borrar solo instalaciones transformadores y seccionadores sin hijos
                if (id_icono_padre < 14)// || id_icono_padre > 21)//13
                {
                    MessageBox.Show(string.Format("No esta permitido borrar la instalación: {0}", nombre), "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Get the count of the child tree nodes contained in the SelectedNode.
                int myNodeCount = targetNode.GetNodeCount(true);
                if (myNodeCount > 0)
                {
                    MessageBox.Show(string.Format("la instalación {0}, no puede tener nodos conectados: {1}", nombre, myNodeCount), "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(string.Format("Seguro de borrar el Nodo: {0}, Instalación: {1}", id_padre, nombre), "Notificación", MessageBoxButtons.YesNo);

                   if (result == DialogResult.Yes)
                                {
                                    if (treeviewModel.UpdateRoot(id_padre, 0, id_icono_padre))
                                    {
                                        targetNode.Remove();
                                    }
                                    else
                                    {
                                        System.Media.SystemSounds.Beep.Play();
                                        MessageBox.Show(string.Format("No se logro borrar la instalación: {0}", nombre), "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                     }

                   
                
            }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.ToString());

             }
        }

        private void cmdContinuo_Click(object sender, EventArgs e)
        {
            mnuActContinuo.PerformClick();
        }

        private void tsbCadMarcarCheck_Click(object sender, EventArgs e)
        {
            mnuActMarcar.PerformClick();
        }

        private void mnuCadBloqCod_Click(object sender, EventArgs e)
        {
            if (mnuOrden.Checked == false || this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Minimized;

            string cod=cadModel.BloqueCodificar();
            if (cod.Length<5)           
                lblObs.Text = "Código de Error: " + cod;            
            else
                lblObs.Text = "Código asignado: " + cod;

            if (mnuOrden.Checked == false || this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.TopMost = true;
            }

        }

        private void mnuEsc1_5_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1_5.Text;
            //if (!cadModel.bloq_esc(1.5))
            //    lblObs.Text = "Función cancelada";
            //else
            //    if (mnuActContinuo.Checked)
            //        mnuEsc1_5.PerformClick();  
        }

        private void tsbEsc1_0_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = tsbCadEsc1_0.Text;           
        }

        private void tsbCadEsc1_2_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = tsbCadEsc1_2.Text;    
        }

        private void tsbCadEsc1_5_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = tsbCadEsc1_5.Text;    
        }

        private void tsbCadEsc_ButtonClick(object sender, EventArgs e)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            //if (!cadModel.bloq_esc(double.Parse(tsbCadEsc.Text)))
            if (!cadModel.bloq_esc(Convert.ToDouble(tsbCadEsc.Text.Replace(",", "."), provider)))            
                lblObs.Text = "Función cancelada";
            else
            {
                if (mnuActContinuo.Checked)
                    tsbCadEsc.PerformButtonClick();
            } 
        }

        private void mnuEditorArbol_Click(object sender, EventArgs e)
        {
            this.TopLevel = false;

            //Fun.FormularioCerrar(typeof(frmSub));
            //Fun.FormularioCerrar(typeof(frmTransf));
            //Fun.FormularioCerrar(typeof(frmCto));

            frmRed f_Arbol = (frmRed)Fun.AbrirFormulario(typeof(frmRed), true);

            

            //f_localidad.FormClosed += Logout;
            this.TopLevel = true;
        }

        private void AbrirPanel(bool b_panel)
        {
            this.Cursor = Cursors.WaitCursor;
            TreeNode node = this.treeView.SelectedNode;

            switch (node.Level)
            {
                case 1://abrir datos de la S/E
                    frmSub.ZOOM = 20;
                    frmSub.ONPANEL = false;
                    SubModel.ID = short.Parse(node.Name);
                    if (b_panel)
                    {

                        frmSub f_sub = (frmSub)Fun.AbrirFormularioInPanel(typeof(frmSub), splitContainer1.Panel2);
                        //f_sub.CargarSE(node.Name);
                    }
                    else
                    {
                        UIHelpers.OpenModule(this, typeof(frmSub), null, true);
                    }
                    break;
                case 2:
                    frmTransf.ZOOM = 10;
                    frmTransf.ONPANEL = false;
                    TransfModel.ID = short.Parse(node.Name);
                     if (b_panel)
                    {
                    frmTransf f_transf = (frmTransf)Fun.AbrirFormularioInPanel(typeof(frmTransf), splitContainer1.Panel2);
                    //f_transf.Cargar(node.Name);
                         }
                    else
                    {
                        UIHelpers.OpenModule(this, typeof(frmTransf), null, true);
                    }
                    break;
                case 3:
                    frmCto.ZOOM = 5;
                    frmCto.ONPANEL = false;
                    CtoModel.ID = short.Parse(node.Name);
                     if (b_panel)
                    {
                    frmCto f_cto = (frmCto)Fun.AbrirFormularioInPanel(typeof(frmCto), splitContainer1.Panel2);
                    //f_cto.Cargar(node.Name);
                           }
                    else
                    {
                        UIHelpers.OpenModule(this, typeof(frmCto), null, true);
                    }
                    break;
                default:
                    if (Int16.Parse(node.ImageIndex.ToString()) > 21)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        string nombre = treeviewModel.SelectNombre(int.Parse(node.Name), Int16.Parse(node.ImageIndex.ToString()));
                        if (!nombre.Equals(null))
                        {
                            UIHelpers.OpenModule(this, typeof(frmClientes), null, true);
                            //this.WindowState = FormWindowState.Minimized;
                        }
                    }
                    break;
            }

            this.Cursor = Cursors.Default;

        }

        private void Logout(object sender, FormClosedEventArgs e)
        {            
            this.TopLevel = true;           
        }
        private void mnuCargarNodos_Click(object sender, EventArgs e)
        {
            mnuCargarNodos.Checked = !mnuCargarNodos.Checked;
        }
        private void mnuCargarForm_Click(object sender, EventArgs e)
        {
            mnuCargarForm.Checked = !mnuCargarForm.Checked;
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOrden_Click(object sender, EventArgs e)
        {
            mnuOrden.PerformClick();
        }

        private void mnuMapVer_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView.SelectedNode;

            if (node.Level < 1)
            {
                System.Media.SystemSounds.Beep.Play();
                lblObs.Text = "Opción no opermitida";
                return;
            }

            double[] SelXY = new double[2];
            SelXY = treeviewModel.SelectXY(int.Parse(node.Name), Int16.Parse(node.ImageIndex.ToString()));

            if (SelXY[0].Equals(0))
            {
                System.Media.SystemSounds.Beep.Play();
                lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }

            cadModel.X = SelXY[0];
            cadModel.Y = SelXY[1];

            cadModel.CoordMap();
            lblObs.Text = "";

        }

        private void btnMapVer_Click(object sender, EventArgs e)
        {
            mnuMapVer.PerformClick();
        }

        private void mnuUbicarTramo_Click(object sender, EventArgs e)
        {
            string codigo = Microsoft.VisualBasic.Interaction.InputBox("Favor Ingrese el Código del TRAMO = ", "Ubicar Nodo en Árbol de la Red", this.txtCodigo.Text, this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
            UbicarNodo(codigo, "ECADSZ01");           
        }

        private void mnuUbicarCarga_Click(object sender, EventArgs e)
        {
            string codigo = Microsoft.VisualBasic.Interaction.InputBox("Favor Ingrese el Código de la Carga = ", "Ubicar Nodo en Árbol de la Red", this.txtCodigo.Text, this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
            UbicarNodo(codigo, "ECADTR01"); 
        }

        private void mnuAbrirFormulario_Click(object sender, EventArgs e)
        {
            
            TreeNode node = this.treeView.SelectedNode;
            if (node.Level > 0 && node.Level < 4 || short.Parse(node.ImageIndex.ToString()) > 21)
            {
                AbrirPanel(false);
            }
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnZoom.PerformClick();
        }

        private void mnuXyMap_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            if (cadModel.CoordObtener())
                cadModel.CoordMap();
            else
                lblObs.Text = cadModel.MENSAJE;      
        }

        private void mnuConvCoor_Click(object sender, EventArgs e)
        {
            this.TopLevel = false;
            frmConvCoord f_ConvCoord = (frmConvCoord)Fun.AbrirFormulario(typeof(frmConvCoord), true);
            this.TopLevel = true;

            
        }

       

        

       

      

      
       

       

       
      

       

       

      


    }


}
